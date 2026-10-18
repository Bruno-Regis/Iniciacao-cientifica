using DoctorPilot.Application.Exceptions;
using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.ConfirmarAgendamento
{
    public class ConfirmarAgendamentoHandler : ICommandHandler<ConfirmarAgendamentoCommand, ResultViewModel<Guid>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ConfirmarAgendamentoHandler(
            IAgendamentoRepository agendamentoRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(
            ConfirmarAgendamentoCommand command,
            CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(command.Id);

            if (agendamento is null)
                throw new NotFoundException("Agendamento não encontrado.");

            if (!_currentUser.IsSuperAdmin && agendamento.ClinicaId != _currentUser.ClinicaId)
            {
                return ResultViewModel<Guid>.Error("Agendamento não encontrado.");
            }

            if (agendamento.Status == StatusAgendamentoEnum.cancelado)
                throw new ConflictException("Não é possível confirmar um agendamento cancelado.");

            if (agendamento.Confirmacao)
                throw new ConflictException("Agendamento já está confirmado.");

            agendamento.Confirmar();
            await _agendamentoRepository.UpdateAsync(agendamento);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(agendamento.Id);
        }
    }
}
