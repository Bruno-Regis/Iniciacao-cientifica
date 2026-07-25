using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Anamneses;
using Mediator;
namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAllAnamnesesByPaciente
{
    public class GetAllAnamnesesByPacienteQuery : IQuery<ResultViewModel<IEnumerable<AnamneseItemDto>>>
    {
        public GetAllAnamnesesByPacienteQuery(Guid pacienteId, int page = 1, int pageSize = 20)
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
