using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.AnamneseTemplates;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAllAnamneseTemplatesByClinica
{
    public class GetAllAnamneseTemplatesByClinicaHandler : IQueryHandler<GetAllAnamneseTemplatesByClinicaQuery, ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>>
    {
        private readonly IAnamneseTemplateRepository _templateRepository;

        public GetAllAnamneseTemplatesByClinicaHandler(IAnamneseTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>> Handle(GetAllAnamneseTemplatesByClinicaQuery query, CancellationToken cancellationToken)
        {
            var templates = await _templateRepository
                .GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);

            var viewModels = templates
                .Select(t => t.ToItemDto());

            return ResultViewModel<IEnumerable<AnamneseTemplateItemDto>>.Success(viewModels);
        }
    }
}
