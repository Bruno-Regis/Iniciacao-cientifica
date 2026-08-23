using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Commands.AlergiasCommands.InsertAlergias
{
    public class InsertAlergiaHandler : ICommandHandler<InsertAlergiaCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public InsertAlergiaHandler(IRepository<Alergia> alergiaRepository, IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _alergiaRepository = alergiaRepository;
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAlergiaCommand command, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var alergia = command.ToEntity();

            await _alergiaRepository.AddAsync(alergia);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(alergia.Id);
        }
    }
}
