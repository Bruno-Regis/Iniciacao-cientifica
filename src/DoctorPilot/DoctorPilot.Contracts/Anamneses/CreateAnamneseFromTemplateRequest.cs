namespace DoctorPilot.Contracts.Anamneses
{
    public class CreateAnamneseFromTemplateRequest
    {
        public Guid PacienteId { get; set; }
        public Guid TemplateId { get; set; }
        public string? Observacoes { get; set; }
        public List<CreateAnamneseFromTemplateRespostaRequest> Respostas { get; set; } = [];
    }

    public class CreateAnamneseFromTemplateRespostaRequest
    {
        public Guid PerguntaId { get; set; }
        public string? Resposta { get; set; }
        public List<Guid> OpcaoIds { get; set; } = [];
    }
}
