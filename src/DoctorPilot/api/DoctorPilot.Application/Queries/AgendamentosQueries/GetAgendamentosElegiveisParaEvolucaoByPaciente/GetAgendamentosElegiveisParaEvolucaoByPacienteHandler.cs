using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAgendamentosElegiveisParaEvolucaoByPaciente
{
    public class GetAgendamentosElegiveisParaEvolucaoByPacienteHandler
        : IQueryHandler<GetAgendamentosElegiveisParaEvolucaoByPacienteQuery, ResultViewModel<IEnumerable<AgendamentoElegivelEvolucaoDto>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAgendamentosElegiveisParaEvolucaoByPacienteHandler(
            IAgendamentoRepository agendamentoRepository,
            IPacienteRepository pacienteRepository,
            ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoElegivelEvolucaoDto>>> Handle(
            GetAgendamentosElegiveisParaEvolucaoByPacienteQuery query,
            CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(query.PacienteId);
            if (paciente is null)
                throw new NotFoundException("Paciente não encontrado.");

            if (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId)
                return ResultViewModel<IEnumerable<AgendamentoElegivelEvolucaoDto>>.Error("Paciente não encontrado.");

            Guid? clinicaId = _currentUser.IsSuperAdmin ? null : _currentUser.ClinicaId;
            var agendamentos = await _agendamentoRepository.GetElegiveisParaEvolucaoByPacienteIdAsync(
                query.PacienteId,
                clinicaId,
                cancellationToken);

            return ResultViewModel<IEnumerable<AgendamentoElegivelEvolucaoDto>>.Success(
                agendamentos.Select(a => a.ToElegivelEvolucaoDto()));
        }
    }
}
