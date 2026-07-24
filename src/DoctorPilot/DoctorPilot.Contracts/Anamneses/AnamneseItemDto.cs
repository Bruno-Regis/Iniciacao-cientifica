namespace DoctorPilot.Contracts.Anamneses
{
    public class AnamneseItemDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeDaAnamnese { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
