namespace DoctorPilot.Contracts.Alergias
{
    public class AlergiaDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public string NomeAlergia { get; set; }
        public string Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }
    }
}
