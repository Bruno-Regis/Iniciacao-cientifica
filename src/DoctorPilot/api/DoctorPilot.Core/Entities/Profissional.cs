namespace DoctorPilot.Core.Entities
{
    public class Profissional : BaseEntity
    {
        public Profissional(Guid clinicaId, string nome, string profissao, string fotoUrl)
        {
            ClinicaId = clinicaId;
            Nome = nome;
            Profissao = profissao;
            FotoUrl = fotoUrl;
        }

        public Guid ClinicaId { get; set; }
        public string Nome { get; set; }
        public string Profissao { get; set; }
        public string FotoUrl { get; set; }
        public string? Crm { get; set; }

        public void SetCrm(string crm)
        {
            Crm = crm;
        }
    }
}
