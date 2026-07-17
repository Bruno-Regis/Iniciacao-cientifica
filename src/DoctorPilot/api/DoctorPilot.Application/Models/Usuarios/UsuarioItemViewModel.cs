using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Usuarios
{
    public class UsuarioItemViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public static UsuarioItemViewModel FromEntity(Usuario usuario)
            => new()
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Role = usuario.Role.ToString(),
                IsActive = usuario.IsActive
            };
    }
}
