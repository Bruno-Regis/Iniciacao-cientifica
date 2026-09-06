using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class Usuario : BaseEntity
    {
        public Usuario(Guid? clinicaId, string email, string senha, RoleEnum role)
        {
            if (role == RoleEnum.SuperAdmin && clinicaId.HasValue)
                throw new ArgumentException("SuperAdmin não deve estar vinculado a uma clínica.", nameof(clinicaId));

            if (role != RoleEnum.SuperAdmin && !clinicaId.HasValue)
                throw new ArgumentException("Usuários tenant devem estar vinculados a uma clínica.", nameof(clinicaId));

            ClinicaId = clinicaId;
            Email = email;
            Senha = senha;
            Role = role;
            UltimoAcesso = DateTime.UtcNow;
            IsActive = true;
        }

        public Guid? ClinicaId { get; set; }
        public Clinica? Clinica { get; set; }
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
