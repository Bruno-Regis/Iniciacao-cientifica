using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class Alergia : BaseEntity
    {
        public Alergia(Guid pacienteId, string nomeAlergia, GravidadeEnum gravidade, DateTime? dataIdentificacao, string? observacoes)
        {
            PacienteId = pacienteId;
            NomeAlergia = nomeAlergia;
            Gravidade = gravidade;
            DataIdentificacao = dataIdentificacao;
            Observacoes = observacoes;
        }

        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        public string NomeAlergia { get; set; }
        public GravidadeEnum Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }
    }
}
