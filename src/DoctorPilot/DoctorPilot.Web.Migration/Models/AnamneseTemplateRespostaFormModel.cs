namespace DoctorPilot.Web.Migration.Models;

public sealed class AnamneseTemplateRespostaFormModel
{
    public Guid PerguntaId { get; set; }
    public string? Resposta { get; set; }
    public HashSet<Guid> OpcaoIds { get; set; } = [];
}
