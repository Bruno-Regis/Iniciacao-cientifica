using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Anamneses
{
    public class AnamneseDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeDaAnamnese { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AnamneseSessaoViewModel> Sessoes { get; set; }

        public static AnamneseDetailsViewModel FromEntity(Anamnese anamnese)
            => new()
            {
                Id = anamnese.Id,
                PacienteId = anamnese.PacienteId,
                ProfissionalId = anamnese.ProfissionalId,
                NomeDaAnamnese = anamnese.NomeDaAnamnese,
                Observacoes = anamnese.Observacoes,
                CreatedAt = anamnese.CreatedAt,
                Sessoes = anamnese.Sessoes.Select(AnamneseSessaoViewModel.FromEntity).ToList()
            };
    }
}
