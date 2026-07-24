using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.AnamneseTemplates;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById
{
    public class GetAnamneseTemplateByIdHandler : IQueryHandler<GetAnamneseTemplateByIdQuery, ResultViewModel<AnamneseTemplateDetailsDto>>
    {
        private readonly IAnamneseTemplateRepository _templateRepository;

        public GetAnamneseTemplateByIdHandler(IAnamneseTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }
        public async ValueTask<ResultViewModel<AnamneseTemplateDetailsDto>> Handle(GetAnamneseTemplateByIdQuery query, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetByIdComSessoesAsync(query.Id);

            if (template is null)
                return ResultViewModel<AnamneseTemplateDetailsDto>.Error("Template não encontrado.");

            return ResultViewModel<AnamneseTemplateDetailsDto>.Success(template.ToDetailsDto());
        }
    }
}
