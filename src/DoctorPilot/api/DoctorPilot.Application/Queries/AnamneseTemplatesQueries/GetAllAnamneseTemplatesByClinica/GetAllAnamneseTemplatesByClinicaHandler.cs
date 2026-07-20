using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.AnamneseTemplates;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAllAnamneseTemplatesByClinica
{
    public class GetAllAnamneseTemplatesByClinicaHandler : IQueryHandler<GetAllAnamneseTemplatesByClinicaQuery, ResultViewModel<IEnumerable<AnamneseTemplateItemViewModel>>>
    {
        private readonly IAnamneseTemplateRepository _templateRepository;

        public GetAllAnamneseTemplatesByClinicaHandler(IAnamneseTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async ValueTask<ResultViewModel<IEnumerable<AnamneseTemplateItemViewModel>>> Handle(GetAllAnamneseTemplatesByClinicaQuery query, CancellationToken cancellationToken)
        {
            var templates = await _templateRepository
                .GetByClinicaIdAsync(query.ClinicaId, query.Page, query.PageSize);

            var viewModels = templates
                .Select(AnamneseTemplateItemViewModel.FromEntity);

            return ResultViewModel<IEnumerable<AnamneseTemplateItemViewModel>>.Success(viewModels);
        }
    }
}
