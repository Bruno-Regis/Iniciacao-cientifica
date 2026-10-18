using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Repositories;
using Mediator;
using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Mappings;
using DoctorPilot.Application.Services;
using DoctorPilot.Core.Enums;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.UpdateEvolucoes
{
    public class UpdateEvolucaoHandler : ICommandHandler<UpdateEvolucaoCommand, ResultViewModel<Guid>>
    {
        private readonly IEvolucaoRepository _evolucaoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateEvolucaoHandler(IEvolucaoRepository evolucaoRepository, IUnitOfWork unitOfWork, IAgendamentoRepository agendamentoRepository, IProfissionalRepository profissionalRepository, ICurrentUserService currentUser)
        {
            _evolucaoRepository = evolucaoRepository;
            _unitOfWork = unitOfWork;
            _agendamentoRepository = agendamentoRepository;
            _profissionalRepository = profissionalRepository;
            _currentUser = currentUser;
        }
        public async ValueTask<ResultViewModel<Guid>> Handle(UpdateEvolucaoCommand command, CancellationToken cancellationToken)
        {
            var evolucao = await _evolucaoRepository.GetByIdAsync(command.Id);
            if (evolucao is null)
                throw new NotFoundException("Evolução não encontrada.");

            var agendamento = await _agendamentoRepository.GetByIdAsync(evolucao.AgendamentoId);
            if (agendamento is null || (!_currentUser.IsSuperAdmin && agendamento.ClinicaId != _currentUser.ClinicaId))
                return ResultViewModel<Guid>.Error("Evolução não encontrada.");

            var profissional = await _profissionalRepository.GetByUsuarioIdAsync(
                _currentUser.UsuarioId,
                cancellationToken);
            if (profissional is null ||
                profissional.ClinicaId != agendamento.ClinicaId ||
                profissional.Id != agendamento.ProfissionalId)
            {
                throw new ForbiddenException("Você não tem permissão para alterar a evolução deste agendamento.");
            }

            if (agendamento.Status == StatusAgendamentoEnum.cancelado)
                throw new ConflictException("Não é possível editar uma evolução de um agendamento cancelado.");

            if (string.IsNullOrWhiteSpace(command.Conteudo))
                return ResultViewModel<Guid>.Error("Conteúdo da evolução é obrigatório.");

            var conteudo = command.Conteudo.Trim();
            if (conteudo.Length > 4000)
                return ResultViewModel<Guid>.Error("Conteúdo da evolução deve ter no máximo 4000 caracteres.");

            evolucao.Conteudo = conteudo;

            await _evolucaoRepository.UpdateAsync(evolucao);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(evolucao.Id);
        }
    }
}
