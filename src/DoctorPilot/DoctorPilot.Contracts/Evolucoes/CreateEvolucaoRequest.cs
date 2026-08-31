namespace DoctorPilot.Contracts.Evolucoes
{
    public class CreateEvolucaoRequest
    {
        public Guid AgendamentoId { get; set; }
        public string Conteudo { get; set; } = string.Empty;
    }
}
