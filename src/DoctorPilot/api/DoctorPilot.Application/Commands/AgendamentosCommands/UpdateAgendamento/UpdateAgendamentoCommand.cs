using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Enums;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.UpdateAgendamento
{
    public class UpdateAgendamentoCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public TipoAgendamentoEnum Tipo { get; set; }
        public string Procedimento { get; set; } = string.Empty;
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string? Sala { get; set; }
        public bool Confirmacao { get; set; }
        public string? Descricao { get; set; }
    }
}
