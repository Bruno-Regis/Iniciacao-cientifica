namespace DoctorPilot.Core.Entities
{
    public class AnamneseTemplateOpcao : BaseEntity
    {
        public AnamneseTemplateOpcao(Guid anamneseTemplateSessaoId, string texto, int ordem)
        {
            AnamneseTemplateSessaoId = anamneseTemplateSessaoId;
            Texto = texto;
            Ordem = ordem;
        }

        public Guid AnamneseTemplateSessaoId { get; set; }
        public AnamneseTemplateSessao AnamneseTemplateSessao { get; set; } = default!;
        public string Texto { get; set; }
        public int Ordem { get; set; }
    }
}
