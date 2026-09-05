namespace DoctorPilot.Web.Migration.Models;

public sealed class AnamneseTemplateSessaoFormModel
{
    public string Pergunta { get; set; } = string.Empty;
    public string Tipo { get; set; } = "texto";
    public bool Obrigatoria { get; set; }
    public List<AnamneseTemplateOpcaoFormModel> Opcoes { get; set; } = [];
}
