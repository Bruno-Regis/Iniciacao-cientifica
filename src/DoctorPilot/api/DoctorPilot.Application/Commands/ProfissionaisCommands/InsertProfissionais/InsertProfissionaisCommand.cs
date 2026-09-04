using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais
{
    public class InsertProfissionaisCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }

        public Profissional ToEntity(Guid clinicaId)
            => new(clinicaId, UsuarioId, Nome, Profissao, FotoUrl, Crm);
    }
}
