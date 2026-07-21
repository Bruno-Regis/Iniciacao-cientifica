using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Anamneses;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAnamneseById
{
    public class GetAnamneseByIdHandler : IQueryHandler<GetAnamneseByIdQuery, ResultViewModel<AnamneseDetailsViewModel>>
    {
        private readonly IAnamneseRepository _anamneseRepository;

        public GetAnamneseByIdHandler(IAnamneseRepository anamneseRepository)
        {
            _anamneseRepository = anamneseRepository;
        }
        public async ValueTask<ResultViewModel<AnamneseDetailsViewModel>> Handle(GetAnamneseByIdQuery query, CancellationToken cancellationToken)
        {
            var anamnese = await _anamneseRepository.GetByIdComSessoesAsync(query.Id);

            if (anamnese is null)
                return ResultViewModel<AnamneseDetailsViewModel>.Error("Anamnese não encontrada.");

            return ResultViewModel<AnamneseDetailsViewModel>.Success(AnamneseDetailsViewModel.FromEntity(anamnese));
        }
    }
}
