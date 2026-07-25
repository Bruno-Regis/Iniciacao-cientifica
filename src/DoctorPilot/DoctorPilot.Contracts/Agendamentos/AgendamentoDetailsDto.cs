namespace DoctorPilot.Contracts.Agendamentos
{
    public class AgendamentoDetailsDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public Guid ClinicaId { get; set; }
        public string Status { get; set; }
        public string Tipo { get; set; }
        public string Procedimento { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string? Sala { get; set; }
        public bool Confirmacao { get; set; }
        public string? Descricao { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
