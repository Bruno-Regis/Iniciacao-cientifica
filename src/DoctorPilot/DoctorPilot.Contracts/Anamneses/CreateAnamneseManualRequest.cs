namespace DoctorPilot.Contracts.Anamneses
{
    public class CreateAnamneseManualRequest
    {
        public Guid PacienteId { get; set; }
        public string NomeDaAnamnese { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public List<CreateAnamneseManualSessaoRequest> Sessoes { get; set; } = [];
    }

    public class CreateAnamneseManualSessaoRequest
    {
        public string Pergunta { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public string? Resposta { get; set; }
        public List<CreateAnamneseManualOpcaoRequest> Opcoes { get; set; } = [];
    }

    public class CreateAnamneseManualOpcaoRequest
    {
        public string Texto { get; set; } = string.Empty;
        public int Ordem { get; set; }
        public bool Selecionada { get; set; }
    }
}
