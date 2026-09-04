namespace DoctorPilot.Contracts.AnamneseTemplates
{
    public class AnamneseTemplateSessaoDto
    {
        public Guid Id { get; set; }
        public string Pergunta { get; set; }
        public string Tipo { get; set; }
        public int Ordem { get; set; }
        public bool Obrigatoria { get; set; }
        public List<AnamneseTemplateOpcaoDto> Opcoes { get; set; } = [];
    }
}
