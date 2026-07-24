using DoctorPilot.Contracts.Pacientes;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class PacienteMappings
    {
        public static PacienteDetailsDto ToDetailsDto(this Paciente paciente)
            => new()
            {
                Id = paciente.Id,
                ClinicaId = paciente.ClinicaId,
                Nome = paciente.Nome,
                Status = paciente.Status.ToString(),
                DataNascimento = paciente.DataNascimento,
                Genero = paciente.Genero.ToString(),
                EstadoCivil = paciente.EstadoCivil.ToString(),
                NomeDaMae = paciente.NomeDaMae,
                NomeDoPai = paciente.NomeDoPai,
                Cpf = paciente.Cpf,
                Rg = paciente.Rg,
                OrgaoExpedidor = paciente.OrgaoExpedidor,
                Telefone = paciente.Telefone,
                Celular = paciente.Celular,
                Email = paciente.Email,
                CEP = paciente.CEP,
                Cidade = paciente.Cidade,
                Estado = paciente.Estado,
                Bairro = paciente.Bairro,
                Numero = paciente.Numero,
                Complemento = paciente.Complemento,
                Profissao = paciente.Profissao,
                NecessidadeEspecial = paciente.NecessidadeEspecial.ToString(),
                Observacoes = paciente.Observacoes,
                CreatedAt = paciente.CreatedAt
            };

        public static PacienteItemDto ToItemDto(this Paciente paciente)
            => new()
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Email = paciente.Email,
                Celular = paciente.Celular,
                Status = paciente.Status.ToString()
            };
    }
}
