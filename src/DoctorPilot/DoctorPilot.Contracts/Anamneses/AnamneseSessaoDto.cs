namespace DoctorPilot.Contracts.Anamneses
{
    public class AnamneseSessaoDto
    {
        public Guid Id { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public string Tipo { get; set; }
    }
}
