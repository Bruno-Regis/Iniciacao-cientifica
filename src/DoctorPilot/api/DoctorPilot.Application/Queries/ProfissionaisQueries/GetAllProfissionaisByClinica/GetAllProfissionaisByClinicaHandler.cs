using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.ProfissionaisQueries.GetAllProfissionaisByClinica
{
    public class GetAllProfissionaisByClinicaHandler : IQueryHandler<GetAllProfissionaisByClinicaQuery, ResultViewModel<IEnumerable<ProfissionalItemDto>>>
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAllProfissionaisByClinicaHandler(IProfissionalRepository profissionalRepository, ICurrentUserService currentUser)
        {
            _profissionalRepository = profissionalRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<IEnumerable<ProfissionalItemDto>>> Handle(GetAllProfissionaisByClinicaQuery query, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUser.IsSuperAdmin ? query.ClinicaId : _currentUser.ClinicaId; // If the user is a super admin, use the provided ClinicaId; otherwise, use the ClinicaId from the current user context.

            var profissionais = await _profissionalRepository.GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);
            var viewModels = profissionais.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<ProfissionalItemDto>>.Success(viewModels);
        }
    }
}
