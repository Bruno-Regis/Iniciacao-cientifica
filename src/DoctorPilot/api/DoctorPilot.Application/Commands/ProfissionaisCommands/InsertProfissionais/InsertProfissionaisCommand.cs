using DoctorPilot.Application.Models;
using DoctorPilot.Core.Entities;
using Mediator;

namespace DoctorPilot.Application.Commands.ProfissionaisCommands.InsertProfissionais
{
    public class InsertProfissionaisCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid ClinicaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }

        public Profissional ToEntity()
            => new(ClinicaId, UsuarioId, Nome, Profissao, FotoUrl, Crm);
    }
}
