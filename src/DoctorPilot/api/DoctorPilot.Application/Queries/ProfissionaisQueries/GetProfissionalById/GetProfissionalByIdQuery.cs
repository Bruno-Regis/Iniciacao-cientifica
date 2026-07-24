using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using Mediator;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById
{
    public class GetProfissionalByIdQuery : IQuery<ResultViewModel<ProfissionalDetailsDto>>
    {
        public GetProfissionalByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
