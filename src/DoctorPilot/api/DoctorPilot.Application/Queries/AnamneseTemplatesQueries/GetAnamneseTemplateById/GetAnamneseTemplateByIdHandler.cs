using DoctorPilot.Contracts.Common;
using DoctorPilot.Contracts.AnamneseTemplates;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Queries.AnamneseTemplatesQueries.GetAnamneseTemplateById
{
    public class GetAnamneseTemplateByIdHandler : IQueryHandler<GetAnamneseTemplateByIdQuery, ResultViewModel<AnamneseTemplateDetailsDto>>
    {
        private readonly IAnamneseTemplateRepository _templateRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAnamneseTemplateByIdHandler(IAnamneseTemplateRepository templateRepository, ICurrentUserService currentUser)
        {
            _templateRepository = templateRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<AnamneseTemplateDetailsDto>> Handle(GetAnamneseTemplateByIdQuery query, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetByIdComSessoesAsync(query.Id, cancellationToken);

            if (template is null || (!_currentUser.IsSuperAdmin && template.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<AnamneseTemplateDetailsDto>.Error("Template não encontrado.");

            return ResultViewModel<AnamneseTemplateDetailsDto>.Success(template.ToDetailsDto());
        }
    }
}
