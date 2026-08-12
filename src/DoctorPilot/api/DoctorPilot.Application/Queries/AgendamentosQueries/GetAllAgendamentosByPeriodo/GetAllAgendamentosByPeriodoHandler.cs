using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo
{
    public class GetAllAgendamentosByPeriodoHandler : IQueryHandler<GetAllAgendamentosByPeriodoQuery, ResultViewModel<IEnumerable<AgendamentoItemDto>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ICurrentUserService _currentUser;
        public GetAllAgendamentosByPeriodoHandler(IAgendamentoRepository agendamentoRepository, ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoItemDto>>> Handle(GetAllAgendamentosByPeriodoQuery query, CancellationToken cancellationToken)
        {

            var clinicaId = _currentUser.IsSuperAdmin ? query.ClinicaId : _currentUser.ClinicaId; // If the user is a super admin, use the provided ClinicaId; otherwise, use the ClinicaId from the current user context.

            var agendamentos = await _agendamentoRepository
                .GetByPeriodoAsync(query.ClinicaId, query.Inicio, query.Fim, query.ProfissionalId);

            var viewModels = agendamentos.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Success(viewModels);
        }
    }
}
