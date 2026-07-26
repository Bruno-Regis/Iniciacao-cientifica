using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Pacientes;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.SearchPacientes
{
    public class SearchPacientesQuery : IQuery<ResultViewModel<IEnumerable<PacienteItemDto>>>
    {
        public SearchPacientesQuery(Guid clinicaId, string termo, int page = 1, int pageSize = 20)
        {
            ClinicaId = clinicaId;
            Termo = termo;
            Page = page;
            PageSize = pageSize;
        }

        public Guid ClinicaId { get; set; }
        public string Termo { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
