namespace DoctorPilot.Web.Migration.Models;

public sealed class AnamneseManualFormModel
{
    public string NomeDaAnamnese { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public List<AnamneseManualSessaoFormModel> Sessoes { get; set; } = [];
}
