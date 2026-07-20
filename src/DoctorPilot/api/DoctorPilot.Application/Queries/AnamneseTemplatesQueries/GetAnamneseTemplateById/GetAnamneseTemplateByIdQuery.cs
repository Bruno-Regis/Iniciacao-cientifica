using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.AnamneseTemplates;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById
{
    public class GetAnamneseTemplateByIdQuery : IQuery<ResultViewModel<AnamneseTemplateDetailsViewModel>>
    {
        public GetAnamneseTemplateByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
