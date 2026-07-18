using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Agendamentos;
using Mediator;

namespace DoctorPilot.Application.Queries.AgendamentosQueries.GetAllAgendamentosByPaciente
{
    public class GetAllAgendamentosByPacienteQuery : IQuery<ResultViewModel<IEnumerable<AgendamentoItemViewModel>>>
    {
        public GetAllAgendamentosByPacienteQuery(Guid pacienteId, int page = 1, int pageSize = 20)
        {
            PacienteId = pacienteId;
            Page = page;
            PageSize = pageSize;
        }

        public Guid PacienteId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
