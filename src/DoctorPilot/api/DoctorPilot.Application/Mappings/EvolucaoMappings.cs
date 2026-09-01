using DoctorPilot.Contracts.Evolucoes;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class EvolucaoMappings
    {
        public static EvolucaoDto ToDto(this Evolucao evolucao)
            => new()
            {
                Id = evolucao.Id,
                AgendamentoId = evolucao.AgendamentoId,
                Conteudo = evolucao.Conteudo,
                DataHoraInicial = evolucao.Agendamento.DataHoraInicial,
                DataHoraFinal = evolucao.Agendamento.DataHoraFinal,
                ProfissionalNome = evolucao.Agendamento.Profissional.Nome,
                Procedimento = evolucao.Agendamento.Procedimento,
                Tipo = evolucao.Agendamento.Tipo.ToString(),
                Status = evolucao.Agendamento.Status.ToString(),
                CreatedAt = evolucao.CreatedAt,
                UpdatedAt = evolucao.UpdatedAt
            };
    }
}
