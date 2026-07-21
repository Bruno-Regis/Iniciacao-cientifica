using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Evolucoes;
using Mediator;

namespace DoctorPilot.Application.Queries.EvolucoesQueries.GetEvolucoesByPaciente
{
    public class GetEvolucoesByPacienteQuery : IQuery<ResultViewModel<IEnumerable<EvolucaoViewModel>>>
    {
        public GetEvolucoesByPacienteQuery(Guid pacienteId, int page = 1, int pageSize = 20)
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
