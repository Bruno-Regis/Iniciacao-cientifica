using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.EvolucoesCommands.InsertEvolucoes
{
    public class InsertEvolucaoCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; }

        public Evolucao ToEntity()
            => new(AgendamentoId, Conteudo);
    }
}
