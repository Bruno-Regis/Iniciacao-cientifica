using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.InsertEvolucoes
{
    public class InsertEvolucaoCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; } = string.Empty;

        public Evolucao ToEntity()
            => new(AgendamentoId, Conteudo.Trim());
    }
}
