using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.PacientesCommands.InsertPacientes
{
    public class InsertPacientesHandler : ICommandHandler<InsertPacientesCommand, ResultViewModel<Guid>>
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public InsertPacientesHandler(IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertPacientesCommand command, CancellationToken cancellationToken)
        {
            var cpfJaCadastrado = await _pacienteRepository.GetByCpfAsync(command.Cpf);
            if (cpfJaCadastrado is not null)
                return ResultViewModel<Guid>.Error("Já existe um paciente cadastrado com este CPF.");

            var paciente = command.ToEntity(_currentUser.ClinicaId);

            await _pacienteRepository.AddAsync(paciente);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(paciente.Id);
        }
    }
}
