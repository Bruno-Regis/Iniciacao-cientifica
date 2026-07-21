using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Evolucoes
{
    public class EvolucaoViewModel
    {
        public Guid Id { get; set; }
        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static EvolucaoViewModel FromEntity(Evolucao evolucao)
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
