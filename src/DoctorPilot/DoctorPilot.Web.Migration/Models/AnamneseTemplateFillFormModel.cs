namespace DoctorPilot.Web.Migration.Models;

public sealed class AnamneseTemplateFillFormModel
{
    public Guid TemplateId { get; set; }
    public string? Observacoes { get; set; }
    public List<AnamneseTemplateRespostaFormModel> Respostas { get; set; } = [];
}
