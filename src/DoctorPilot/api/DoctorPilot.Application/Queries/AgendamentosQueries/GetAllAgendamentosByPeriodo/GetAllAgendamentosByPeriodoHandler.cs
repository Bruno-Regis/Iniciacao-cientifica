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
            if (query.Fim <= query.Inicio)
                return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Error("A data/hora final deve ser posterior à data/hora inicial.");

            var agendamentos = await _agendamentoRepository
                .GetByPeriodoAsync(_currentUser.GetRequiredClinicaId(), query.Inicio, query.Fim, query.ProfissionalId, cancellationToken);

            var viewModels = agendamentos.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Success(viewModels);
        }
    }
}
