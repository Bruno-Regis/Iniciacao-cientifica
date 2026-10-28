namespace DoctorPilot.Contracts.Agendamentos
{
    public class AgendamentoDetailsDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public Guid ClinicaId { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public string ProfissionalNome { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Procedimento { get; set; } = string.Empty;
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string? Sala { get; set; }
        public bool Confirmacao { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
