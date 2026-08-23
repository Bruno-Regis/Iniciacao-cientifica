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
            var clinicaId = _currentUser.ClinicaId; 
            var profissionais = await _profissionalRepository.GetByClinicaIdAsync(clinicaId, query.Page, query.PageSize);
            var viewModels = profissionais.Select(p => p.ToItemDto());
            return ResultViewModel<IEnumerable<ProfissionalItemDto>>.Success(viewModels);
        }
    }
}
