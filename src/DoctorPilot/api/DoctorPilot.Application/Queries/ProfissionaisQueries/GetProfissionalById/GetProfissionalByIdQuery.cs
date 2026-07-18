using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Profissionais;
using Mediator;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById
{
    public class GetProfissionalByIdQuery : IQuery<ResultViewModel<ProfissionalDetailsViewModel>>
    {
        public GetProfissionalByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
