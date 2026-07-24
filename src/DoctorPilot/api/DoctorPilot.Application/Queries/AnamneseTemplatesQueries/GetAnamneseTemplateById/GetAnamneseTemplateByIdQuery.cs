using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.AnamneseTemplates;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById
{
    public class GetAnamneseTemplateByIdQuery : IQuery<ResultViewModel<AnamneseTemplateDetailsDto>>
    {
        public GetAnamneseTemplateByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
