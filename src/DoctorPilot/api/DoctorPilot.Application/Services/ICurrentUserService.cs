
namespace DoctorPilot.Application.Services
{
    public interface ICurrentUserService
    {
        Guid UsuarioId { get; }
        Guid ClinicaId { get; }
        string Role { get; }
        bool IsSuperAdmin { get; }
    }
}
