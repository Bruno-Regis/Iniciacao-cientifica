using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPaciente
{
    public class GetAllAgendamentosByPacienteHandler : IQueryHandler<GetAllAgendamentosByPacienteQuery, ResultViewModel<IEnumerable<AgendamentoItemDto>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAllAgendamentosByPacienteHandler(IAgendamentoRepository agendamentoRepository, IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoItemDto>>> Handle(GetAllAgendamentosByPacienteQuery query, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(query.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Error("Paciente não encontrado.");

            var agendamentos = await _agendamentoRepository.GetByPacienteIdAsync(query.PacienteId, query.Page, query.PageSize, cancellationToken);
            var viewModels = agendamentos.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Success(viewModels);
        }
    }
}
