using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Anamneses;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAnamneseById
{
    public class GetAnamneseByIdQuery : IQuery<ResultViewModel<AnamneseDetailsDto>>
    {
        public GetAnamneseByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
