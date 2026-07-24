using DoctorPilot.Contracts.Usuarios;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class UsuarioMappings
    {
        public static UsuarioDetailsDto ToDetailsDto(this Usuario usuario)
            => new()
            {
                Id = usuario.Id,
                ClinicaId = usuario.ClinicaId,
                Email = usuario.Email,
                Role = usuario.Role.ToString(),
                IsActive = usuario.IsActive,
                UltimoAcesso = usuario.UltimoAcesso,
                CreatedAt = usuario.CreatedAt
            };

        public static UsuarioItemDto ToItemDto(this Usuario usuario)
            => new()
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Role = usuario.Role.ToString(),
                IsActive = usuario.IsActive
            };
    }
}
