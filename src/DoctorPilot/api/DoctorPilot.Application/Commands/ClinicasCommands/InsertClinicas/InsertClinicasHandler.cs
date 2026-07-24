using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Commands.ClinicasCommands.InsertClinicas
{
    public class InsertClinicasHandler : ICommandHandler<InsertClinicasCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<Clinica> _clinicaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertClinicasHandler(IRepository<Clinica> clinicaRepository, IUnitOfWork unitOfWork)
        {
            _clinicaRepository = clinicaRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertClinicasCommand command, CancellationToken cancellationToken)
        {
            var clinica = command.ToEntity();

            await _clinicaRepository.AddAsync(clinica);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(clinica.Id);
        }
    }
}
