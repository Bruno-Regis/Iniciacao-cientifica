using DoctorPilot.Application.Models;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.InsertEvolucoes
{
    public class InsertEvolucaoHandler : ICommandHandler<InsertEvolucaoCommand, ResultViewModel<Guid>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertEvolucaoHandler(IEvolucaoRepository evolucaoRepository,IAgendamentoRepository agendamentoRepository,IUnitOfWork unitOfWork)
        {
            _evolucaoRepository = evolucaoRepository;
            _agendamentoRepository = agendamentoRepository;
            _unitOfWork = unitOfWork;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertEvolucaoCommand command, CancellationToken cancellationToken)
        {
            var agendamentoExiste = await _agendamentoRepository.ExistsAsync(command.AgendamentoId);
            if (!agendamentoExiste)
                return ResultViewModel<Guid>.Error("Agendamento não encontrado.");

            var evolucaoExistente = await _evolucaoRepository.GetByAgendamentoIdAsync(command.AgendamentoId);
            if (evolucaoExistente is not null)
                return ResultViewModel<Guid>.Error("Já existe uma evolução para este agendamento.");

            var evolucao = command.ToEntity();

            await _evolucaoRepository.AddAsync(evolucao);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(evolucao.Id);
        }
    }
}
