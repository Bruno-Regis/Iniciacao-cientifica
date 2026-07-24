using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.AnamneseTemplates;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAllAnamneseTemplatesByClinica
{
    public class GetAllAnamneseTemplatesByClinicaQuery : IQuery<ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>>
    {
        public GetAllAnamneseTemplatesByClinicaQuery(Guid clinicaId, int page = 1, int pageSize = 20)
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
