using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.InsertEvolucoes
{
    public class InsertEvolucaoHandler : ICommandHandler<InsertEvolucaoCommand, ResultViewModel<Guid>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public InsertEvolucaoHandler(IEvolucaoRepository evolucaoRepository, IAgendamentoRepository agendamentoRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _evolucaoRepository = evolucaoRepository;
            _agendamentoRepository = agendamentoRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(InsertEvolucaoCommand command, CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(command.AgendamentoId);
            if (agendamento is null)
                throw new NotFoundException("Agendamento não encontrado.");

            if (!_currentUser.IsSuperAdmin && agendamento.ClinicaId != _currentUser.ClinicaId)
                return ResultViewModel<Guid>.Error("Agendamento não encontrado.");

            if (agendamento.Status == StatusAgendamentoEnum.cancelado)
                throw new ConflictException("Não é possível registrar evolução para um agendamento cancelado.");

            if (string.IsNullOrWhiteSpace(command.Conteudo))
                return ResultViewModel<Guid>.Error("Conteúdo da evolução é obrigatório.");

            if (command.Conteudo.Trim().Length > 4000)
                return ResultViewModel<Guid>.Error("Conteúdo da evolução deve ter no máximo 4000 caracteres.");

            var evolucaoExistente = await _evolucaoRepository.GetByAgendamentoIdAsync(
                command.AgendamentoId,
                cancellationToken);
            if (evolucaoExistente is not null)
                throw new ConflictException("Já existe uma evolução para este agendamento.");

            var evolucao = command.ToEntity();

            await _evolucaoRepository.AddAsync(evolucao);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(evolucao.Id);
        }
    }
}
