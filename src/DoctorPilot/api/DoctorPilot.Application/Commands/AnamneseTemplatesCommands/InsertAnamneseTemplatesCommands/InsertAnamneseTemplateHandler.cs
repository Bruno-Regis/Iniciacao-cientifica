using DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.InsertAnamneseTemplate;
using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.InsertAnamneseTemplatesCommands.AnamneseTemplatesCommands
{
    public class InsertAnamneseTemplateHandler : ICommandHandler<InsertAnamneseTemplateCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<AnamneseTemplate> _templateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertAnamneseTemplateHandler(IRepository<AnamneseTemplate> templateRepository, IUnitOfWork unitOfWork)
        {
            _templateRepository = templateRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAnamneseTemplateCommand command, CancellationToken cancellationToken)
        {
            var template = command.ToEntity();

            await _templateRepository.AddAsync(template);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(template.Id);
        }
    }
}
