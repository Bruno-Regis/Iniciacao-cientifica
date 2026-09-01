namespace DoctorPilot.Core.Entities
{
    public class Evolucao : BaseEntity
    {
        public Evolucao(Guid agendamentoId, string conteudo)
        {
            AgendamentoId = agendamentoId;
            Conteudo = conteudo;
        }

        public Guid AgendamentoId { get; set; }
        public Agendamento Agendamento { get; set; } = null!;
        public string Conteudo { get; set; }
    }
}
