using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
 
namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById
{
    public class GetProfissionalByIdHandler : IQueryHandler<GetProfissionalByIdQuery, ResultViewModel<ProfissionalDetailsDto>>
    {
        private readonly IProfissionalRepository _profissionalRepository;

        public GetProfissionalByIdHandler(IProfissionalRepository profissionalRepository)
        {
            _profissionalRepository = profissionalRepository;
        }

        public async ValueTask<ResultViewModel<ProfissionalDetailsDto>> Handle(GetProfissionalByIdQuery query, CancellationToken cancellationToken)
        {
            var profissional = await _profissionalRepository.GetByUsuarioIdAsync(query.Id);

            if (profissional is null)
                return ResultViewModel<ProfissionalDetailsDto>.Error("Profissional n�o encontrado.");

            return ResultViewModel<ProfissionalDetailsDto>.Success(profissional.ToDetailsDto());
        }
    }
}