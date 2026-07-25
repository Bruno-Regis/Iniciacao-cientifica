namespace DoctorPilot.Contracts.AnamneseTemplates
{
    public class AnamneseTemplateDetailsDto
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<AnamneseTemplateSessaoDto> Sessoes { get; set; }
    }
}
