using DoctorPilot.Contracts.AnamneseTemplates;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class AnamneseTemplateMappings
    {
        public static AnamneseTemplateDetailsDto ToDetailsDto(this AnamneseTemplate template)
            => new()
            {
                Id = template.Id,
                ClinicaId = template.ClinicaId,
                Nome = template.Nome,
                Descricao = template.Descricao,
                Sessoes = template.Sessoes.Select(s => s.ToDto()).ToList()
            };

        public static AnamneseTemplateItemDto ToItemDto(this AnamneseTemplate template)
            => new()
            {
                Id = template.Id,
                ClinicaId = template.ClinicaId,
                Nome = template.Nome,
                Descricao = template.Descricao
            };

        public static AnamneseTemplateSessaoDto ToDto(this AnamneseTemplateSessao sessao)
            => new()
            {
                Id = sessao.Id,
                Pergunta = sessao.Pergunta,
                Tipo = sessao.Tipo.ToString()
            };
    }
}
