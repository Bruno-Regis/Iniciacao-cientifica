namespace DoctorPilot.Contracts.Profissionais;

public class CreateProfissionalRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Profissao { get; set; } = string.Empty;
    public string? FotoUrl { get; set; }
    public string? Crm { get; set; }
}
