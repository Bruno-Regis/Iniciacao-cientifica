using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class Alergia : BaseEntity
    {
        public Guid PacienteId { get; set; }
        public string NomeAlergia { get; set; }
        public GravidadeEnum Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }
    }
}
