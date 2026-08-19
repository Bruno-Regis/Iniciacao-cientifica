using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.GetAllPacientesByClinica
{
    public class GetAllPacientesByClinicaQuery : IQuery<ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        public GetAllPacientesByClinicaQuery(int page = 1, int pageSize = 20)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}