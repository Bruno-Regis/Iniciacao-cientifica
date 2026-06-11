using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class Usuario : BaseEntity
    {
        public Usuario(Guid clinicaId, string email, string senha, RoleEnum role)
        {
            ClinicaId = clinicaId;
            Email = email;
            Senha = senha;
            Role = role;
            UltimoAcesso = DateTime.UtcNow;
            IsActive = true;
        }

        public Guid ClinicaId { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public RoleEnum Role { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public bool IsActive { get; set; }

        public void AtualizarUltimoAcesso()
        {
            UltimoAcesso = DateTime.UtcNow;
        }

        public void DesligarUsuario()
        {
            IsActive = false;
        }

        public void AtivarUsuario()
        {
            IsActive = true;
        }
    }
}
