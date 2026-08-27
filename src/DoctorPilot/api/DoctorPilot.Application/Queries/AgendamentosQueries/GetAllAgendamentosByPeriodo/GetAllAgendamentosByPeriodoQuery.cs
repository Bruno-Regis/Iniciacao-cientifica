using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo
{
    public class GetAllAgendamentosByPeriodoQuery : IQuery<ResultViewModel<IEnumerable<AgendamentoItemDto>>>
    {
        public GetAllAgendamentosByPeriodoQuery(DateTime inicio, DateTime fim, Guid? profissionalId = null)
        {
            Inicio = inicio;
            Fim = fim;
            ProfissionalId = profissionalId;
        }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public Guid? ProfissionalId { get; set; }

    }
}
