using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais
{
    public class InsertProfissionaisCommand : ICommand<ResultViewModel<Guid>>
    {
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Profissao { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }

        public Profissional ToEntity(Guid clinicaId, Guid usuarioId)
            => new(clinicaId, usuarioId, Nome, Profissao, FotoUrl, Crm);
    }
}
