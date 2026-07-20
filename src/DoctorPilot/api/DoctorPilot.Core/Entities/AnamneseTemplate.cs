namespace DoctorPilot.Core.Entities
{
    public class AnamneseTemplate : BaseEntity
    {
        public AnamneseTemplate(Guid clinicaId, string nome, string? descricao)
        {
            ClinicaId = clinicaId;
            Nome = nome;
            Descricao = descricao;
            Sessoes = [];
        }

        public Guid ClinicaId { get; set; }
        public Clinica Clinica { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public List<AnamneseTemplateSessao> Sessoes { get; set; }
    }
}
