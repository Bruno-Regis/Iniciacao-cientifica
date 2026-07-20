using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Models.Alergias
{
    public class AlergiaViewModel
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public string NomeAlergia { get; set; }
        public string Gravidade { get; set; }
        public DateTime? DataIdentificacao { get; set; }
        public string? Observacoes { get; set; }

        public static AlergiaViewModel FromEntity(Alergia alergia)
            => new()
            {
                Id = alergia.Id,
                PacienteId = alergia.PacienteId,
                NomeAlergia = alergia.NomeAlergia,
                Gravidade = alergia.Gravidade.ToString(),
                DataIdentificacao = alergia.DataIdentificacao,
                Observacoes = alergia.Observacoes
            };
    }
}
