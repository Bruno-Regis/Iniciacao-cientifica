using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetAllPacientesByClinica
{
    public class GetAllPacientesByClinicaQuery : IQuery<ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        public GetAllPacientesByClinicaQuery(Guid clinicaId, int page = 1, int pageSize = 20)
        {
            ClinicaId = clinicaId;
            Page = page;
            PageSize = pageSize;
        }

        public Guid ClinicaId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}