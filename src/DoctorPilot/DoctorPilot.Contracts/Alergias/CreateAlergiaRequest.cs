namespace DoctorPilot.Contracts.Alergias
{
    public class CreateAlergiaRequest
    {
        public Guid PacienteId { get; set; }
        public string NomeAlergia { get; set; } = string.Empty;
        public int Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }
    }
}
