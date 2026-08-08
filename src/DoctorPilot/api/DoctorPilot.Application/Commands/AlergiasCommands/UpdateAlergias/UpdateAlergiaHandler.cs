using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AlergiasCommands.UpdateAlergias
{
    public class UpdateAlergiaHandler : ICommandHandler<UpdateAlergiaCommand, ResultViewModel<Guid>>
    {
        private readonly IRepository<Alergia> _alergiaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAlergiaHandler(IRepository<Alergia> alergiaRepository, IUnitOfWork unitOfWork)
        {
            _alergiaRepository = alergiaRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(UpdateAlergiaCommand command, CancellationToken cancellationToken)
        {
            var alergia = await _alergiaRepository.GetByIdAsync(command.AlergiaId); 

            if (alergia == null)
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