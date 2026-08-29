using DoctorPilot.Application.Services;
using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.Repositories;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.CancelarAgendamento
{
    public class CancelarAgendamentoHandler : ICommandHandler<CancelarAgendamentoCommand, ResultViewModel<Guid>>
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CancelarAgendamentoHandler(
            IAgendamentoRepository agendamentoRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _agendamentoRepository = agendamentoRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async ValueTask<ResultViewModel<Guid>> Handle(
            CancelarAgendamentoCommand command,
            CancellationToken cancellationToken)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(command.Id);

            if (agendamento is null ||
                (!_currentUser.IsSuperAdmin && agendamento.ClinicaId != _currentUser.ClinicaId))
            {
                return ResultViewModel<Guid>.Error("Agendamento não encontrado.");
            }

            if (agendamento.Status == StatusAgendamentoEnum.cancelado)
                return ResultViewModel<Guid>.Error("Agendamento já está cancelado.");

            agendamento.Cancelar();
            await _agendamentoRepository.UpdateAsync(agendamento);
            await _unitOfWork.CommitAsync();

            return ResultViewModel<Guid>.Success(agendamento.Id);
        }
    }
}
