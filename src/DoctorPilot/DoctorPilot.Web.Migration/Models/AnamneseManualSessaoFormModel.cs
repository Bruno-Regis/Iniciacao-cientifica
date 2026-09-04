namespace DoctorPilot.Web.Migration.Models;

public sealed class AnamneseManualSessaoFormModel
{
    public string Pergunta { get; set; } = string.Empty;
    public string Tipo { get; set; } = "texto";
    public string? Resposta { get; set; }
    public List<AnamneseManualOpcaoFormModel> Opcoes { get; set; } = [];
}
