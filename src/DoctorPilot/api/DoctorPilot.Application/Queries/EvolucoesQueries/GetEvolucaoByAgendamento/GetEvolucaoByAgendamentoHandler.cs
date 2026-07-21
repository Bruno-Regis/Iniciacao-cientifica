using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Evolucoes;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucaoByAgendamento
{
    public class GetEvolucaoByAgendamentoHandler : IQueryHandler<GetEvolucaoByAgendamentoQuery, ResultViewModel<EvolucaoViewModel>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;

        public GetEvolucaoByAgendamentoHandler(IEvolucaoRepository evolucaoRepository)
        {
            _evolucaoRepository = evolucaoRepository;
        }
        public async ValueTask<ResultViewModel<EvolucaoViewModel>> Handle(GetEvolucaoByAgendamentoQuery query, CancellationToken cancellationToken)
        {
            var evolucao = await _evolucaoRepository.GetByAgendamentoIdAsync(query.AgendamentoId);

            if (evolucao is null)
                return ResultViewModel<EvolucaoViewModel>.Error("Evolução não encontrada para este agendamento.");

            return ResultViewModel<EvolucaoViewModel>.Success(EvolucaoViewModel.FromEntity(evolucao));
        }
    }
}
