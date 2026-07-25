using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Commands.AnamneseCommands.InsertAnamneseCommands
{
    public class InsertAnamneseHandler : ICommandHandler<InsertAnamneseCommand, ResultViewModel<Guid>>
    {
        private readonly IAnamneseRepository _anamneseRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertAnamneseHandler(IAnamneseRepository anamneseRepository,IPacienteRepository pacienteRepository,IUnitOfWork unitOfWork)
        {
            _anamneseRepository = anamneseRepository;
            _pacienteRepository = pacienteRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertAnamneseCommand command, CancellationToken cancellationToken)
        {
            var pacienteExiste = await _pacienteRepository.ExistsAsync(command.PacienteId);
            if (!pacienteExiste)
                return ResultViewModel<Guid>.Error("Paciente não encontrado.");

            var anamnese = command.ToEntity();

            await _anamneseRepository.AddAsync(anamnese);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(anamnese.Id);
        }
    }
}
