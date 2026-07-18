using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Agendamentos;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo
{
    public class GetAllAgendamentosByPeriodoHandler : IQueryHandler<GetAllAgendamentosByPeriodoQuery, ResultViewModel<IEnumerable<AgendamentoItemViewModel>>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public GetAllAgendamentosByPeriodoHandler(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }
        public async ValueTask<ResultViewModel<IEnumerable<AgendamentoItemViewModel>>> Handle(GetAllAgendamentosByPeriodoQuery query, CancellationToken cancellationToken)
        {
            var agendamentos = await _agendamentoRepository.GetByPeriodoAsync(query.Inicio, query.Fim);
            var viewModels = agendamentos.Select(AgendamentoItemViewModel.FromEntity);
            return ResultViewModel<IEnumerable<AgendamentoItemViewModel>>.Success(viewModels);
        }
    }
}
