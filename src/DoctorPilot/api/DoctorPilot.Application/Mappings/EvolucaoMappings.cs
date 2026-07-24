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
                CreatedAt = evolucao.CreatedAt,
                UpdatedAt = evolucao.UpdatedAt
            };
    }
}
