using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Alergias;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AlergiasQueries.GetAlergiaById
{
    public class GetAlergiaByIdHandler : IQueryHandler<GetAlergiaByIdQuery, ResultViewModel<AlergiaViewModel>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;

        public GetAlergiaByIdHandler(IRepository<Alergia> alergiaRepository)
        {
            _alergiaRepository = alergiaRepository;
        }
        public async ValueTask<ResultViewModel<AlergiaViewModel>> Handle(GetAlergiaByIdQuery query, CancellationToken cancellationToken)
        {
            var alergia = await _alergiaRepository.GetByIdAsync(query.Id);

            if (alergia is null)
                return ResultViewModel<AlergiaViewModel>.Error("Alergia não encontrada.");

            return ResultViewModel<AlergiaViewModel>.Success(AlergiaViewModel.FromEntity(alergia));
        }
    }
}
