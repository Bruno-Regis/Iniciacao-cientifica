namespace DoctorPilot.Contracts.AnamneseTemplates
{
    public class AnamneseTemplateOpcaoDto
    {
        public Guid Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public int Ordem { get; set; }
    }
}
