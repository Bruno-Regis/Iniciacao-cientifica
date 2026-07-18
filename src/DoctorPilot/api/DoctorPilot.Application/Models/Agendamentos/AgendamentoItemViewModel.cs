using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Agendamentos
{
    public class AgendamentoItemViewModel
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string Status { get; set; }
        public string Tipo { get; set; }
        public string Procedimento { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string? Sala { get; set; }
        public bool Confirmacao { get; set; }

        public static AgendamentoItemViewModel FromEntity(Agendamento agendamento)
            => new()
            {
                Id = agendamento.Id,
                PacienteId = agendamento.PacienteId,
                ProfissionalId = agendamento.ProfissionalId,
                Status = agendamento.Status.ToString(),
                Tipo = agendamento.Tipo.ToString(),
                Procedimento = agendamento.Procedimento,
                DataHoraInicial = agendamento.DataHoraInicial,
                DataHoraFinal = agendamento.DataHoraFinal,
                Sala = agendamento.Sala,
                Confirmacao = agendamento.Confirmacao
            };
    }
}
