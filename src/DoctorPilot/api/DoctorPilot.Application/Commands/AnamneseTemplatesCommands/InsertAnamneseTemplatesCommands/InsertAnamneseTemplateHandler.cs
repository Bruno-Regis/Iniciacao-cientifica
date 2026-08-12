using DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.InsertAnamneseTemplate;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.AnamneseTemplatesCommands
{
    public class InsertAnamneseTemplateHandler : ICommandHandler<InsertAnamneseTemplateCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<AnamneseTemplate> _templateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public InsertAnamneseTemplateHandler(IRepository<AnamneseTemplate> templateRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _templateRepository = templateRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAnamneseTemplateCommand command, CancellationToken cancellationToken)
        {
            var template = command.ToEntity(_currentUser.ClinicaId);

            await _templateRepository.AddAsync(template);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(template.Id);
        }
    }
}
