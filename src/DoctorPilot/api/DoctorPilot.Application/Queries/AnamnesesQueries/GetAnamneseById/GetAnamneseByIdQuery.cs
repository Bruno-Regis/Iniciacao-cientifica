using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Anamneses;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAnamneseById
{
    public class GetAnamneseByIdQuery : IQuery<ResultViewModel<AnamneseDetailsViewModel>>
    {
        public GetAnamneseByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
