using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AlergiasCommands.InsertAlergias
{
    public class InsertAlergiaHandler : ICommandHandler<InsertAlergiaCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertAlergiaHandler(IRepository<Alergia> alergiaRepository,IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork)
        {
            _alergiaRepository = alergiaRepository;
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAlergiaCommand command, CancellationToken cancellationToken)
        {
            var pacienteExiste = await _pacienteRepository.ExistsAsync(command.PacienteId);
            if (!pacienteExiste)
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var alergia = command.ToEntity();

            await _alergiaRepository.AddAsync(alergia);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(alergia.Id);
        }
    }
}
