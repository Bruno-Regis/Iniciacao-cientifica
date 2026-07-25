using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Mappings;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.UpdateEvolucoes
{
    public class UpdateEvolucaoHandler : ICommandHandler<UpdateEvolucaoCommand, ResultViewModel<Guid>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEvolucaoHandler(IEvolucaoRepository evolucaoRepository, IUnitOfWork unitOfWork)
        {
            _evolucaoRepository = evolucaoRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(UpdateEvolucaoCommand command, CancellationToken cancellationToken)
        {
            var evolucao = await _evolucaoRepository.GetByIdAsync(command.Id);
            if (evolucao is null)
                return ResultViewModel<Guid>.Error("Evolução não encontrada.");

            evolucao.Conteudo = command.Conteudo;

            await _evolucaoRepository.UpdateAsync(evolucao);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(evolucao.Id);
        }
    }
}
