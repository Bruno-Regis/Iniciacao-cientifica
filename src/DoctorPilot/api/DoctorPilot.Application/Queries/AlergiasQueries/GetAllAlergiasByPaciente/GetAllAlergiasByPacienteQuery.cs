using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Alergias;
using Mediator;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAllAlergiasByPaciente
{
    public class GetAllAlergiasByPacienteQuery : IQuery<ResultViewModel<IEnumerable<AlergiaDto>>>
    {
        public GetAllAlergiasByPacienteQuery(Guid pacienteId)
        {
            PacienteId = pacienteId;
        }

        public Guid PacienteId { get; set; }
    }
}
