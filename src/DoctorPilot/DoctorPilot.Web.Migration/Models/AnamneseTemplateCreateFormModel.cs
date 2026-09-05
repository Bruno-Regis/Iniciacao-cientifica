namespace DoctorPilot.Web.Migration.Models;

public sealed class AnamneseTemplateCreateFormModel
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public List<AnamneseTemplateSessaoFormModel> Sessoes { get; set; } = [];
}
