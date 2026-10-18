using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.InsertAgendamentos
{
    public class InsertAgendamentoHandler : ICommandHandler<InsertAgendamentoCommand, ResultViewModel<Guid>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;

        public InsertAgendamentoHandler(IAgendamentoRepository agendamentoRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, IPacienteRepository pacienteRepository, IProfissionalRepository profissionalRepository)
        {
            _agendamentoRepository = agendamentoRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _pacienteRepository = pacienteRepository;
            _profissionalRepository = profissionalRepository;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAgendamentoCommand command, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUser.GetRequiredClinicaId();

            if (command.DataHoraFinal <= command.DataHoraInicial)
                return ResultViewModel<Guid>.Error("A data/hora final deve ser posterior à data/hora inicial.");

            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);
            if (paciente is null)
                throw new NotFoundException("Paciente não encontrado.");

            if (paciente.ClinicaId != clinicaId)
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var profissional = await _profissionalRepository.GetByIdAsync(command.ProfissionalId);
            if (profissional is null)
                throw new NotFoundException("Profissional não encontrado.");

            if (profissional.ClinicaId != clinicaId)
                return ResultViewModel<Guid>.Error("Profissional não encontrado.");

            var conflitoComProfissional = await _agendamentoRepository.ExisteConflitoProfissionalAsync(
                clinicaId,
                command.ProfissionalId,
                command.DataHoraInicial,
                command.DataHoraFinal,
                cancellationToken: cancellationToken);
            if (conflitoComProfissional)
                return ResultViewModel<Guid>.Error("Profissional já possui um agendamento neste período.");

            if (!string.IsNullOrWhiteSpace(command.Sala))
            {
                var conflitoComSala = await _agendamentoRepository.ExisteConflitoSalaAsync(
                    clinicaId,
                    command.Sala,
                    command.DataHoraInicial,
                    command.DataHoraFinal,
                    cancellationToken: cancellationToken);

                if (conflitoComSala)
                    return ResultViewModel<Guid>.Error("Sala já está ocupada neste período.");
            }

            var agendamento = command.ToEntity(clinicaId);
            await _agendamentoRepository.AddAsync(agendamento);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(agendamento.Id);
        }
    }
}

