using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;

namespace DoctorPilot.Application.Commands.AlergiasCommands.UpdateAlergias
{
    public class UpdateAlergiaHandler : ICommandHandler<UpdateAlergiaCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateAlergiaHandler(IRepository<Alergia> alergiaRepository, IUnitOfWork unitOfWork, IPacienteRepository pacienteRepository, ICurrentUserService currentUser)
        {
            _alergiaRepository = alergiaRepository;
            _unitOfWork = unitOfWork;
            _pacienteRepository = pacienteRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(UpdateAlergiaCommand command, CancellationToken cancellationToken)
        {
            var alergia = await _alergiaRepository.GetByIdAsync(command.AlergiaId); 

            if (alergia is null)
                throw new NotFoundException("Alergia não encontrada.");

            var paciente = await _pacienteRepository.GetByIdAsync(alergia.PacienteId);
            if (paciente is null || (!_currentUser.IsSuperAdmin && paciente.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<Guid>.Error("Alergia não encontrada.");
            
          
            alergia.NomeAlergia = command.NomeAlergia;
            alergia.Gravidade = command.Gravidade;
            alergia.DataIdentificacao = command.DataIdentificacao;
            alergia.Observacoes = command.Observacoes;

            await _alergiaRepository.UpdateAsync(alergia);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(alergia.Id);

        }
    }
}
