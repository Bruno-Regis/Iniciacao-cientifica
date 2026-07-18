using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Profissionais
{
    public class ProfissionalDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }
        public Guid ClinicaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Email { get; set; }

        public static ProfissionalDetailsViewModel FromEntity(Profissional profissional)
            => new()
            {
                Id = profissional.Id,
                Nome = profissional.Nome,
                Profissao = profissional.Profissao,
                FotoUrl = profissional.FotoUrl,
                Crm = profissional.Crm,
                ClinicaId = profissional.ClinicaId,
                UsuarioId = profissional.UsuarioId,
                Email = profissional.Usuario?.Email ?? string.Empty
            };
    }
}
