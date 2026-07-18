using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.Profissionais;
using DoctorPilot.Core.Repositories;
using Mediator;
 
namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById
{
    public class GetProfissionalByIdHandler : IQueryHandler<GetProfissionalByIdQuery, ResultViewModel<ProfissionalDetailsViewModel>>
    {
        private readonly IProfissionalRepository _profissionalRepository;

        public GetProfissionalByIdHandler(IProfissionalRepository profissionalRepository)
        {
            _profissionalRepository = profissionalRepository;
        }

        public async ValueTask<ResultViewModel<ProfissionalDetailsViewModel>> Handle(GetProfissionalByIdQuery query, CancellationToken cancellationToken)
        {
            var profissional = await _profissionalRepository.GetByUsuarioIdAsync(query.Id);

            if (profissional is null)
                return ResultViewModel<ProfissionalDetailsViewModel>.Error("Profissional não encontrado.");

            return ResultViewModel<ProfissionalDetailsViewModel>.Success(ProfissionalDetailsViewModel.FromEntity(profissional));
        }
    }
}