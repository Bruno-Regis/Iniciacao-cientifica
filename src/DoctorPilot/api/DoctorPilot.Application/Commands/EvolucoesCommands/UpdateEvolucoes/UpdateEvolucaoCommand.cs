using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.UpdateEvolucoes
{
    public class UpdateEvolucaoCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid Id { get; set; }
        public string Conteudo { get; set; } = string.Empty;
    }
}
