
namespace DoctorPilot.Contracts.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ClinicaId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
 