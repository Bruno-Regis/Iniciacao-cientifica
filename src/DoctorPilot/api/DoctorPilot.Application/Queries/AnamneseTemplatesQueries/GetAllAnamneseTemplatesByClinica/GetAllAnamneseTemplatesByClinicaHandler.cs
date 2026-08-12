using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.AnamneseTemplates;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAllAnamneseTemplatesByClinica
{
    public class GetAllAnamneseTemplatesByClinicaHandler : IQueryHandler<GetAllAnamneseTemplatesByClinicaQuery, ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>>
    {
        private readonly IAnamneseTemplateRepository _templateRepository;
        private readonly ICurrentUserService _currentUser;
        public GetAllAnamneseTemplatesByClinicaHandler(IAnamneseTemplateRepository templateRepository, ICurrentUserService currentUser)
        {
            _templateRepository = templateRepository;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>> Handle(GetAllAnamneseTemplatesByClinicaQuery query, CancellationToken cancellationToken)
        {
            var clinicaId = _currentUser.IsSuperAdmin ? query.ClinicaId : _currentUser.ClinicaId; // If the user is a super admin, use the provided ClinicaId; otherwise, use the ClinicaId from the current user context.

            var templates = await _templateRepository
                .GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);

            var viewModels = templates
                .Select(t => t.ToItemDto());

            return ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>.Success(viewModels);
        }
    }
}
