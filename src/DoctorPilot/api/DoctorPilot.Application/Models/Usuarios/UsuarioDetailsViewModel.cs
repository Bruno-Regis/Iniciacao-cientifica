using DoctorPilot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Application.Models.Usuarios
{
    public class UsuarioDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        public DateTime CreatedAt { get; set; }

        public static UsuarioDetailsViewModel FromEntity(Usuario usuario)
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
    }
}
