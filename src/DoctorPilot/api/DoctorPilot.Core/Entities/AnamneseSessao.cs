using DoctorPilot.Core.Enums;
namespace DoctorPilot.Core.Entities
{
    public class AnamneseSessao : BaseEntity
    {
        public AnamneseSessao(Guid anamneseId, string pergunta, string resposta, AnamneseSessaoTypeEnum tipo)
        {
            AnamneseId = anamneseId;
            Pergunta = pergunta;
            Resposta = resposta;
            Tipo = tipo;
        }

        public Guid AnamneseId { get; set; }
        public Anamnese Anamnese { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public AnamneseSessaoTypeEnum Tipo { get; set; }
    }
}
