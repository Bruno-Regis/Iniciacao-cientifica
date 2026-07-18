using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Pacientes;
using Mediator;

namespace DoctorPilot.Application.Queries.PacientesQueries.SearchPacientes
{
    public class SearchPacientesQuery : IQuery<ResultViewModel<IEnumerable<PacienteItemViewModel>>>
    {
        public SearchPacientesQuery(string termo, int page = 1, int pageSize = 20)
        {
            Termo = termo;
            Page = page;
            PageSize = pageSize;
        }

        public string Termo { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
