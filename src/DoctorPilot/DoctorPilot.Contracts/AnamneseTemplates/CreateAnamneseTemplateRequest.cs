namespace DoctorPilot.Contracts.AnamneseTemplates
{
    public class CreateAnamneseTemplateRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public List<CreateAnamneseTemplateSessaoRequest> Sessoes { get; set; } = [];
    }

    public class CreateAnamneseTemplateSessaoRequest
    {
        public string Pergunta { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public bool Obrigatoria { get; set; }
        public List<CreateAnamneseTemplateOpcaoRequest> Opcoes { get; set; } = [];
    }

    public class CreateAnamneseTemplateOpcaoRequest
    {
        public string Texto { get; set; } = string.Empty;
        public int Ordem { get; set; }
    }
}
