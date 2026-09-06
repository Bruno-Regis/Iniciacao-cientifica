namespace DoctorPilot.Contracts.Usuarios
{
    public class UsuarioDetailsDto
    {
        public Guid Id { get; set; }
        public Guid? ClinicaId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
