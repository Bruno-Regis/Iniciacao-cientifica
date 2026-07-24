namespace DoctorPilot.Contracts.Profissionais
{
    public class ProfissionalItemDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }
    }
}
