using DoctorPilot.Contracts.Anamneses;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class AnamneseMappings
    {
        public static AnamneseDetailsDto ToDetailsDto(this Anamnese anamnese)
            => new()
            {
                Id = anamnese.Id,
                PacienteId = anamnese.PacienteId,
                ProfissionalId = anamnese.ProfissionalId,
                AnamneseTemplateId = anamnese.AnamneseTemplateId,
                NomeDaAnamnese = anamnese.NomeDaAnamnese,
                Observacoes = anamnese.Observacoes,
                CreatedAt = anamnese.CreatedAt,
                Sessoes = anamnese.Sessoes
                    .OrderBy(s => s.Ordem)
                    .ThenBy(s => s.Id)
                    .Select(s => s.ToDto())
                    .ToList()
            };

        public static AnamneseItemDto ToItemDto(this Anamnese anamnese)
            => new()
            {
                Id = anamnese.Id,
                PacienteId = anamnese.PacienteId,
                ProfissionalId = anamnese.ProfissionalId,
                NomeDaAnamnese = anamnese.NomeDaAnamnese,
                CreatedAt = anamnese.CreatedAt
            };

        public static AnamneseSessaoDto ToDto(this AnamneseSessao sessao)
            => new()
            {
                Id = sessao.Id,
                Pergunta = sessao.Pergunta,
                Resposta = sessao.Resposta,
                Tipo = sessao.Tipo.ToString(),
                Ordem = sessao.Ordem,
                Opcoes = sessao.Opcoes
                    .OrderBy(o => o.Ordem)
                    .ThenBy(o => o.Id)
                    .Select(o => new AnamneseSessaoOpcaoDto
                    {
                        Id = o.Id,
                        Texto = o.Texto,
                        Ordem = o.Ordem,
                        Selecionada = o.Selecionada
                    })
                    .ToList()
            };
    }
}
