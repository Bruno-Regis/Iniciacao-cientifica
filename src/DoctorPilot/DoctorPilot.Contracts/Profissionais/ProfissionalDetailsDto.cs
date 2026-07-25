namespace DoctorPilot.Contracts.Profissionais
{
    public class ProfissionalDetailsDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }
        public Guid ClinicaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Email { get; set; }
    }
}
