using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
 
namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetProfissionalById
{
    public class GetProfissionalByIdHandler : IQueryHandler<GetProfissionalByIdQuery, ResultViewModel<ProfissionalDetailsDto>>
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUser;

        public GetProfissionalByIdHandler(IProfissionalRepository profissionalRepository, ICurrentUserService currentUser)
        {
            _profissionalRepository = profissionalRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<ProfissionalDetailsDto>> Handle(GetProfissionalByIdQuery query, CancellationToken cancellationToken)
        {
            var profissional = await _profissionalRepository.GetByIdComRelacionamentosAsync(query.Id, cancellationToken);

            if (profissional is null || (!_currentUser.IsSuperAdmin && profissional.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<ProfissionalDetailsDto>.Error("Profissional não encontrado.");

            return ResultViewModel<ProfissionalDetailsDto>.Success(profissional.ToDetailsDto());
        }
    }
}
