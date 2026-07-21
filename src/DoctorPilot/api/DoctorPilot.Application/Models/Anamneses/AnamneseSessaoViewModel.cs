using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Anamneses
{
    public class AnamneseSessaoViewModel
    {
        public Guid Id { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public string Tipo { get; set; }

        public static AnamneseSessaoViewModel FromEntity(AnamneseSessao sessao)
            => new()
            {
                Id = sessao.Id,
                Pergunta = sessao.Pergunta,
                Resposta = sessao.Resposta,
                Tipo = sessao.Tipo.ToString()
            };
    }
}
