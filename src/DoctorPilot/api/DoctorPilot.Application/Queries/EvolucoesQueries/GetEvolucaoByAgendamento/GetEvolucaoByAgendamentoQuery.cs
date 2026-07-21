using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Evolucoes;
using Mediator;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucaoByAgendamento
{
    public class GetEvolucaoByAgendamentoQuery : IQuery<ResultViewModel<EvolucaoViewModel>>
    {
        public GetEvolucaoByAgendamentoQuery(Guid agendamentoId)
        {
            AgendamentoId = agendamentoId;
        }

        public Guid AgendamentoId { get; set; }
    }
}
