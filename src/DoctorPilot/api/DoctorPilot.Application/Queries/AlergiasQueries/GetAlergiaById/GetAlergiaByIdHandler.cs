using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById
{
    public class GetAlergiaByIdHandler : IQueryHandler<GetAlergiaByIdQuery, ResultViewModel<AlergiaDto>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;

        public GetAlergiaByIdHandler(IRepository<Alergia> alergiaRepository)
        {
            _alergiaRepository = alergiaRepository;
        }
        public async ValueTask<ResultViewModel<AlergiaDto>> Handle(GetAlergiaByIdQuery query, CancellationToken cancellationToken)
        {
            var alergia = await _alergiaRepository.GetByIdAsync(query.Id);

            if (alergia is null)
                return ResultViewModel<AlergiaDto>.Error("Alergia não encontrada.");

            return ResultViewModel<AlergiaDto>.Success(alergia.ToDto());
        }
    }
}
