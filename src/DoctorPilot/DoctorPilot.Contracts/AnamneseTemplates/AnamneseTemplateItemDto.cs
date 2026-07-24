namespace DoctorPilot.Contracts.AnamneseTemplates
{
    public class AnamneseTemplateItemDto
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
    }
}
