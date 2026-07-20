using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class AnamneseTemplateSessao : BaseEntity
    {
        public AnamneseTemplateSessao(Guid anamneseTemplateId, string pergunta, AnamneseSessaoTypeEnum tipo)
        {
            AnamneseTemplateId = anamneseTemplateId;
            Pergunta = pergunta;
            Tipo = tipo;
        }

        public Guid AnamneseTemplateId { get; set; }
        public string Pergunta { get; set; }
        public AnamneseSessaoTypeEnum Tipo { get; set; }
    }
}
