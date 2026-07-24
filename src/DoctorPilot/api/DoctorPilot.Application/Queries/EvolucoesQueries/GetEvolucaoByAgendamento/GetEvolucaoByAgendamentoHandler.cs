using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Evolucoes;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucaoByAgendamento
{
    public class GetEvolucaoByAgendamentoHandler : IQueryHandler<GetEvolucaoByAgendamentoQuery, ResultViewModel<EvolucaoDto>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;

        public GetEvolucaoByAgendamentoHandler(IEvolucaoRepository evolucaoRepository)
        {
            _evolucaoRepository = evolucaoRepository;
        }
        public async ValueTask<ResultViewModel<EvolucaoDto>> Handle(GetEvolucaoByAgendamentoQuery query, CancellationToken cancellationToken)
        {
            var evolucao = await _evolucaoRepository.GetByAgendamentoIdAsync(query.AgendamentoId);

            if (evolucao is null)
                return ResultViewModel<EvolucaoDto>.Error("Evolução não encontrada para este agendamento.");

            return ResultViewModel<EvolucaoDto>.Success(evolucao.ToDto());
        }
    }
}
