namespace DoctorPilot.Core.Entities
{
    public class Anamnese : BaseEntity
    {
        public Anamnese(
            Guid pacienteId,
            Guid profissionalId,
            string nomeDaAnamnese,
            string? observacoes,
            Guid? anamneseTemplateId = null)
        {
            PacienteId = pacienteId;
            ProfissionalId = profissionalId;
            AnamneseTemplateId = anamneseTemplateId;
            NomeDaAnamnese = nomeDaAnamnese;
            Observacoes = observacoes;
            Sessoes = [];
        }

        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public Guid? AnamneseTemplateId { get; set; }
        public AnamneseTemplate? AnamneseTemplate { get; set; }
        public string NomeDaAnamnese { get; set; }
        public string? Observacoes { get; set; }
        public List<AnamneseSessao> Sessoes { get; set; }
    }
}
