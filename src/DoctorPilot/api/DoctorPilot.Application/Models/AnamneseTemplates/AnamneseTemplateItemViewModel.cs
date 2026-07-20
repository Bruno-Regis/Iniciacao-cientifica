using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.AnamneseTemplates
{
    public class AnamneseTemplateItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }

        public static AnamneseTemplateItemViewModel FromEntity(AnamneseTemplate template)
            => new()
            {
                Id = template.Id,
                ClinicaId = template.ClinicaId,
                Nome = template.Nome,
                Descricao = template.Descricao
            };
    } 
}
