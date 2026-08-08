using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using Mediator;

namespace DoctorPilot.Application.Commands.AlergiasCommands.UpdateAlergias
{
    public class UpdateAlergiaCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid AlergiaId { get; set; }
        public string NomeAlergia { get; set; }
        public GravidadeEnum Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }
    }
}
