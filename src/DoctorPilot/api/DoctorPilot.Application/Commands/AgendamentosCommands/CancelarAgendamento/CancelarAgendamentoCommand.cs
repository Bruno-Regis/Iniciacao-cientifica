using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.CancelarAgendamento
{
    public class CancelarAgendamentoCommand : ICommand<ResultViewModel<Guid>>
    {
        public CancelarAgendamentoCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
