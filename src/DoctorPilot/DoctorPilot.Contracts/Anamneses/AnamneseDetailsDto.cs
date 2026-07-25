namespace DoctorPilot.Contracts.Anamneses
{
    public class AnamneseDetailsDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeDaAnamnese { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AnamneseSessaoDto> Sessoes { get; set; }
    }
}
