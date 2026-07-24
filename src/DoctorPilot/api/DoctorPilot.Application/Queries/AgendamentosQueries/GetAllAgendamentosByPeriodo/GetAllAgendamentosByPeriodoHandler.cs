using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo
{
    public class GetAllAgendamentosByPeriodoHandler : IQueryHandler<GetAllAgendamentosByPeriodoQuery, ResultViewModel<IEnumerable<AgendamentoItemDto>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public GetAllAgendamentosByPeriodoHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoItemDto>>> Handle(GetAllAgendamentosByPeriodoQuery query, CancellationToken cancellationToken)
        {
            var agendamentos = await _agendamentoRepository.GetByPeriodoAsync(query.Inicio, query.Fim);
            var viewModels = agendamentos.Select(a => a.ToItemDto());
            return ResultViewModel<IEnumerable<AgendamentoItemDto>>.Success(viewModels);
        }
    }
}
