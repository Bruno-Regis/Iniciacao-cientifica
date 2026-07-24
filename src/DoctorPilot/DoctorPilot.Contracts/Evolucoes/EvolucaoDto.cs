namespace DoctorPilot.Contracts.Evolucoes
{
    public class EvolucaoDto
    {
        public Guid Id { get; set; }
        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
