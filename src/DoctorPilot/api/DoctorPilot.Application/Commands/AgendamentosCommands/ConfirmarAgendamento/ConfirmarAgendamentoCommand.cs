using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.ConfirmarAgendamento
{
    public class ConfirmarAgendamentoCommand : ICommand<ResultViewModel<Guid>>
    {
        public ConfirmarAgendamentoCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
