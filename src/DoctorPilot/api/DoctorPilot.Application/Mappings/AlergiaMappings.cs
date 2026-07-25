using DoctorPilot.Contracts.Alergias;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class AlergiaMappings
    {
        public static AlergiaDto ToDto(this Alergia alergia)
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
