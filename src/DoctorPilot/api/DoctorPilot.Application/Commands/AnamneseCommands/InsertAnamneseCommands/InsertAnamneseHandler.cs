using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseCommands
{
    public class InsertAnamneseHandler : ICommandHandler<InsertAnamneseCommand, ResultViewModel<Guid>>
    {
        private readonly IAnamneseRepository _anamneseRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUser;

        public InsertAnamneseHandler(IAnamneseRepository anamneseRepository, IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork, IProfissionalRepository profissionalRepository, ICurrentUserService currentUser)
        {
            _anamneseRepository = anamneseRepository;
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
            _profissionalRepository = profissionalRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAnamneseCommand command, CancellationToken cancellationToken)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(command.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var profissional = await _profissionalRepository.GetByIdAsync(command.ProfissionalId);
            if (profissional is null || (!_currentUser.IsSuperAdmin && profissional.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<Guid>.Error("Profissional não encontrado.");

            if (profissional.ClinicaId != paciente.ClinicaId)
                return ResultViewModel<Guid>.Error("Paciente e profissional devem pertencer à mesma clínica.");

            var anamnese = command.ToEntity();

            await _anamneseRepository.AddAsync(anamnese);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(anamnese.Id);
        }
    }
}
