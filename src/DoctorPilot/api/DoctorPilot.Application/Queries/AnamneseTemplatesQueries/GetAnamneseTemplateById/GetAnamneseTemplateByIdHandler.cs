using DoctorPilot.Application.Models;
using DoctorPilot.Application.Models.AnamneseTemplates;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById
{
    public class GetAnamneseTemplateByIdHandler : IQueryHandler<GetAnamneseTemplateByIdQuery, ResultViewModel<AnamneseTemplateDetailsViewModel>>
    {
        private readonly IAnamneseTemplateRepository _templateRepository;

        public GetAnamneseTemplateByIdHandler(IAnamneseTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }
        public async ValueTask<ResultViewModel<AnamneseTemplateDetailsViewModel>> Handle(GetAnamneseTemplateByIdQuery query, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetByIdComSessoesAsync(query.Id);

            if (template is null)
                return ResultViewModel<AnamneseTemplateDetailsViewModel>.Error("Template não encontrado.");

            return ResultViewModel<AnamneseTemplateDetailsViewModel>.Success(AnamneseTemplateDetailsViewModel.FromEntity(template));
        }
    }
}
