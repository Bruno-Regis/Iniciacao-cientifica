using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Alergias;
using Mediator;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente
{
    public class GetAllAlergiasByPacienteQuery : IQuery<ResultViewModel<IEnumerable<AlergiaViewModel>>>
    {
        public GetAllAlergiasByPacienteQuery(Guid pacienteId)
        {
            PacienteId = pacienteId;
        }

        public Guid PacienteId { get; set; }
    }
}
