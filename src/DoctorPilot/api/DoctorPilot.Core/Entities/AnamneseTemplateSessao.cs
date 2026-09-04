using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class AnamneseTemplateSessao : BaseEntity
    {
        public AnamneseTemplateSessao(
            Guid anamneseTemplateId,
            string pergunta,
            AnamneseSessaoTypeEnum tipo,
            int ordem = 0,
            bool obrigatoria = false)
        {
            AnamneseTemplateId = anamneseTemplateId;
            Pergunta = pergunta;
            Tipo = tipo;
            Ordem = ordem;
            Obrigatoria = obrigatoria;
            Opcoes = [];
        }

        public Guid AnamneseTemplateId { get; set; }
        public string Pergunta { get; set; }
        public AnamneseSessaoTypeEnum Tipo { get; set; }
        public int Ordem { get; set; }
        public bool Obrigatoria { get; set; }
        public List<AnamneseTemplateOpcao> Opcoes { get; set; }
    }
}
