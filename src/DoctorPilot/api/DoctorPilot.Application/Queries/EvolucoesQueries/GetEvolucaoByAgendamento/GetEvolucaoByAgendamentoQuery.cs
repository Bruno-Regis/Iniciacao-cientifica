using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Evolucoes;
using Mediator;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucaoByAgendamento
{
    public class GetEvolucaoByAgendamentoQuery : IQuery<ResultViewModel<EvolucaoDto>>
    {
        public GetEvolucaoByAgendamentoQuery(Guid agendamentoId)
        {
            AgendamentoId = agendamentoId;
        }

        public Guid AgendamentoId { get; set; }
    }
}
