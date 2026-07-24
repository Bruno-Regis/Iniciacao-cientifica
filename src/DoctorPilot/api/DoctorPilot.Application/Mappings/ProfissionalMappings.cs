using DoctorPilot.Contracts.Profissionais;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class ProfissionalMappings
    {
        public static ProfissionalDetailsDto ToDetailsDto(this Profissional profissional)
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

        public static ProfissionalItemDto ToItemDto(this Profissional profissional)
            => new()
            {
                Id = profissional.Id,
                Nome = profissional.Nome,
                Profissao = profissional.Profissao,
                FotoUrl = profissional.FotoUrl,
                Crm = profissional.Crm
            };
    }
}
