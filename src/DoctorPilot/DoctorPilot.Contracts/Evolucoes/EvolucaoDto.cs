namespace DoctorPilot.Contracts.Evolucoes
{
    public class EvolucaoDto
    {
        public Guid Id { get; set; }
        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string ProfissionalNome { get; set; } = string.Empty;
        public string Procedimento { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
