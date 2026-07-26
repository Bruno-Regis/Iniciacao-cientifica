using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Agendamentos;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPeriodo
{
    public class GetAllAgendamentosByPeriodoQuery : IQuery<ResultViewModel<IEnumerable<AgendamentoItemDto>>>
    {
        public GetAllAgendamentosByPeriodoQuery(Guid clinicaId, DateTime inicio, DateTime fim, Guid? profissionalId = null)
        {
            ClinicaId = clinicaId;
            Inicio = inicio;
            Fim = fim;
            ProfissionalId = profissionalId;
        }

        public Guid ClinicaId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public Guid? ProfissionalId { get; set; }

    }
}
