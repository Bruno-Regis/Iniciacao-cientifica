using DoctorPilot.Application.Services;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAvailableSlots
{
    public class GetAvailableSlotsHandler : IQueryHandler<GetAvailableSlotsQuery, ResultViewModel<IEnumerable<AvailableSlotDto>>>
    {
        private const int SlotIntervalMinutes = 15;

        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IHorarioProfissionalRepository _horarioRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAvailableSlotsHandler(IProfissionalRepository profissionalRepository, IHorarioProfissionalRepository horarioRepository, IAgendamentoRepository agendamentoRepository, ICurrentUserService currentUserService)
        {
            _profissionalRepository = profissionalRepository;
            _horarioRepository = horarioRepository;
            _agendamentoRepository = agendamentoRepository;
            _currentUserService = currentUserService;
        }

        public async ValueTask<ResultViewModel<IEnumerable<AvailableSlotDto>>> Handle(GetAvailableSlotsQuery query, CancellationToken cancellationToken)
        {
            if (query.Fim <= query.Inicio)
                return ResultViewModel<IEnumerable<AvailableSlotDto>>.Error("A data/hora final deve ser posterior à data/hora inicial.");

            if (query.DuracaoMinutos <= 0)
                return ResultViewModel<IEnumerable<AvailableSlotDto>>.Error("A duração deve ser maior que zero.");

            var clinicaId = _currentUserService.GetRequiredClinicaId();

            var profissional = await _profissionalRepository
                .GetByIdAsync(query.ProfissionalId);

            if (profissional is null)
                throw new NotFoundException("Profissional não encontrado.");

            if (profissional.ClinicaId != clinicaId)
                return ResultViewModel<IEnumerable<AvailableSlotDto>>.Error("Profissional não encontrado.");

            var horarios = await _horarioRepository
                .GetByProfissionalIdAsync(
                    query.ProfissionalId,
                    cancellationToken);

            var agendamentos = await _agendamentoRepository
                .GetByPeriodoAsync(
                    clinicaId,
                    query.Inicio,
                    query.Fim,
                    query.ProfissionalId,
                    cancellationToken);

            var slots = new List<AvailableSlotDto>();

            for (var data = query.Inicio.Date; data <= query.Fim.Date; data = data.AddDays(1))
            {
                var horariosDoDia = horarios
                    .Where(h => h.DiaSemana == data.DayOfWeek);

                foreach (var horario in horariosDoDia)
                {
                    var inicioExpediente = data.Add(horario.HoraInicio.ToTimeSpan());
                    var fimExpediente = data.Add(horario.HoraFim.ToTimeSpan());

                    var inicioPermitido =
                        inicioExpediente < query.Inicio
                            ? query.Inicio
                            : inicioExpediente;

                    inicioPermitido = AlinharProximoSlot(inicioPermitido);

                    var fimPermitido =
                        fimExpediente > query.Fim
                            ? query.Fim
                            : fimExpediente;

                    for (var inicioSlot = inicioPermitido;
                         inicioSlot.AddMinutes(query.DuracaoMinutos) <= fimPermitido;
                         inicioSlot = inicioSlot.AddMinutes(SlotIntervalMinutes))
                    {
                        var fimSlot =
                            inicioSlot.AddMinutes(query.DuracaoMinutos);

                        var possuiConflito = agendamentos.Any(a =>
                            a.Agendamento.Status != StatusAgendamentoEnum.cancelado &&
                            a.Agendamento.DataHoraInicial < fimSlot &&
                            a.Agendamento.DataHoraFinal > inicioSlot);

                        if (possuiConflito)
                            continue;

                        slots.Add(new AvailableSlotDto
                        {
                            Inicio = inicioSlot,
                            Fim = fimSlot
                        });
                    }
                }
            }

            return ResultViewModel<IEnumerable<AvailableSlotDto>>.Success(slots);
        }
        private static DateTime AlinharProximoSlot(DateTime dateTime)
        {
            var baseTime = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                0,
                dateTime.Kind);

            var minutosRestantes = baseTime.Minute % SlotIntervalMinutes;

            var estaExatamenteNaGrade =
                minutosRestantes == 0 &&
                dateTime == baseTime;

            if (estaExatamenteNaGrade)
                return baseTime;

            var minutosAdicionar =
                minutosRestantes == 0
                    ? SlotIntervalMinutes
                    : SlotIntervalMinutes - minutosRestantes;

            return baseTime.AddMinutes(minutosAdicionar);
        }
    }
}
