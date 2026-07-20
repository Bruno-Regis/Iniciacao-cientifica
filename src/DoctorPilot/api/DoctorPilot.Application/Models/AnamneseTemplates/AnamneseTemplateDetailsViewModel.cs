using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.AnamneseTemplates
{
    public class AnamneseTemplateDetailsViewModel
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<AnamneseTemplateSessaoViewModel> Sessoes { get; set; }

        public static AnamneseTemplateDetailsViewModel FromEntity(AnamneseTemplate template)
            => new()
            {
                Id = template.Id,
                ClinicaId = template.ClinicaId,
                Nome = template.Nome,
                Descricao = template.Descricao,
                Sessoes = template.Sessoes.Select(AnamneseTemplateSessaoViewModel.FromEntity).ToList()
            };
    }
}
