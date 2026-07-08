namespace DoctorPilot.Core.Entities
{
    public class Transcricao : BaseEntity
    {
        public Transcricao(Guid agendamentoId, string textoTranscrito)
        {
            AgendamentoId = agendamentoId;
            TextoTranscrito = textoTranscrito;
        }

        public Guid AgendamentoId { get; set; }
        public string TextoTranscrito { get; set; }

    }
}
