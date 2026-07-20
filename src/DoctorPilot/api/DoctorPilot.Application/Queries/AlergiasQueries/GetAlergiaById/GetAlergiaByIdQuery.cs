using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Alergias;
using Mediator;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById
{
    public class GetAlergiaByIdQuery : IQuery<ResultViewModel<AlergiaViewModel>>
    {
        public GetAlergiaByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
