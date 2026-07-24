using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Alergias;
using Mediator;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById
{
    public class GetAlergiaByIdQuery : IQuery<ResultViewModel<AlergiaDto>>
    {
        public GetAlergiaByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
