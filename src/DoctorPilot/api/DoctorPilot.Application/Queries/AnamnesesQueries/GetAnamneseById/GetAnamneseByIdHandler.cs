using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Anamneses;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AnamnesesQueries.GetAnamneseById
{
    public class GetAnamneseByIdHandler : IQueryHandler<GetAnamneseByIdQuery, ResultViewModel<AnamneseDetailsDto>>
    {
        private readonly IAnamneseRepository _anamneseRepository;

        public GetAnamneseByIdHandler(IAnamneseRepository anamneseRepository)
        {
            _anamneseRepository = anamneseRepository;
        }
        public async ValueTask<ResultViewModel<AnamneseDetailsDto>> Handle(GetAnamneseByIdQuery query, CancellationToken cancellationToken)
        {
            var anamnese = await _anamneseRepository.GetByIdComSessoesAsync(query.Id);

            if (anamnese is null)
                return ResultViewModel<AnamneseDetailsDto>.Error("Anamnese não encontrada.");

            return ResultViewModel<AnamneseDetailsDto>.Success(anamnese.ToDetailsDto());
        }
    }
}
