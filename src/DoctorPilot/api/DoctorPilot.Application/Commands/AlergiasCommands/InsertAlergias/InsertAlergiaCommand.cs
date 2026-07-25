using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using Mediator;

namespace DoctorPilot.Application.Commands.AlergiasCommands.InsertAlergias
{
    public class InsertAlergiaCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid PacienteId { get; set; }
        public string NomeAlergia { get; set; }
        public GravidadeEnum Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }

        public Alergia ToEntity()
            => new(PacienteId, NomeAlergia, Gravidade, DataIdentificacao, Observacoes);
    }
}
