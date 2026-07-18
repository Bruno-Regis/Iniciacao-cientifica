using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Agendamentos;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo
{
    public class GetAllAgendamentosByPeriodoQuery : IQuery<ResultViewModel<IEnumerable<AgendamentoItemViewModel>>>
    {
        public GetAllAgendamentosByPeriodoQuery(DateTime inicio, DateTime fim)
        {
            Inicio = inicio;
            Fim = fim;
        }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}
