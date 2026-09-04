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
                Sessoes = template.Sessoes
                    .OrderBy(s => s.Ordem)
                    .ThenBy(s => s.Id)
                    .Select(s => s.ToDto())
                    .ToList()
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
                Tipo = sessao.Tipo.ToString(),
                Ordem = sessao.Ordem,
                Obrigatoria = sessao.Obrigatoria,
                Opcoes = sessao.Opcoes
                    .OrderBy(o => o.Ordem)
                    .ThenBy(o => o.Id)
                    .Select(o => new AnamneseTemplateOpcaoDto
                    {
                        Id = o.Id,
                        Texto = o.Texto,
                        Ordem = o.Ordem
                    })
                    .ToList()
            };
    }
}
