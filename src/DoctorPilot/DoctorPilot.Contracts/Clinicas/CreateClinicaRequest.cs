namespace DoctorPilot.Contracts.Clinicas;

public class CreateClinicaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public CreateClinicaAdminRequest Administrador { get; set; } = new();
}
