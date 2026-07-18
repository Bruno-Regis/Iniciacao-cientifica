

using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Profissionais
{
    public class ProfissionalItemViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string? FotoUrl { get; set; }
        public string? Crm { get; set; }

        public static ProfissionalItemViewModel FromEntity(Profissional profissional)
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
