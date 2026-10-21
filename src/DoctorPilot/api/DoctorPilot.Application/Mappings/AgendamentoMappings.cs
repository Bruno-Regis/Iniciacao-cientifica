using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.ReadModels;

namespace DoctorPilot.Application.Mappings
{
    public static class AgendamentoMappings
    {
        public static AgendamentoDetailsDto ToDetailsDto(this Agendamento agendamento)
            => new()
            {
                Id = agendamento.Id,
                PacienteId = agendamento.PacienteId,
                ProfissionalId = agendamento.ProfissionalId,
                ClinicaId = agendamento.ClinicaId,
                Status = agendamento.Status.ToString(),
                Tipo = agendamento.Tipo.ToString(),
                Procedimento = agendamento.Procedimento,
                DataHoraInicial = agendamento.DataHoraInicial,
                DataHoraFinal = agendamento.DataHoraFinal,
                Sala = agendamento.Sala,
                Confirmacao = agendamento.Confirmacao,
                Descricao = agendamento.Descricao,
                CreatedAt = agendamento.CreatedAt
            };

        public static AgendamentoItemDto ToItemDto(this Agendamento agendamento)
            => new()
            {
                Id = agendamento.Id,
                PacienteId = agendamento.PacienteId,
                ProfissionalId = agendamento.ProfissionalId,
                ClinicaId = agendamento.ClinicaId,
                Status = agendamento.Status.ToString(),
                Tipo = agendamento.Tipo.ToString(),
                Procedimento = agendamento.Procedimento,
                DataHoraInicial = agendamento.DataHoraInicial,
                DataHoraFinal = agendamento.DataHoraFinal,
                Sala = agendamento.Sala,
                Confirmacao = agendamento.Confirmacao
            };

        public static AgendamentoItemDto ToItemDto(this AgendamentoComNomes resultado)
        {
            var dto = resultado.Agendamento.ToItemDto();
            dto.PacienteNome = resultado.PacienteNome;
            dto.ProfissionalNome = resultado.ProfissionalNome;
            return dto;
        }

        public static AgendamentoElegivelEvolucaoDto ToElegivelEvolucaoDto(this AgendamentoComNomes resultado)
            => new()
            {
                Id = resultado.Agendamento.Id,
                DataHoraInicial = resultado.Agendamento.DataHoraInicial,
                DataHoraFinal = resultado.Agendamento.DataHoraFinal,
                ProfissionalNome = resultado.ProfissionalNome,
                Procedimento = resultado.Agendamento.Procedimento,
                Tipo = resultado.Agendamento.Tipo.ToString()
            };
        public static AgendamentoPeriodoItemDto ToPeriodoItemDto(this AgendamentoComNomes resultado)
            => new()
            {
                PacienteNome = resultado.PacienteNome,
                ProfissionalNome = resultado.ProfissionalNome,
                Status = resultado.Agendamento.Status.ToString(),
                Tipo = resultado.Agendamento.Tipo.ToString(),
                Procedimento = resultado.Agendamento.Procedimento,
                DataHoraInicial = resultado.Agendamento.DataHoraInicial,
                DataHoraFinal = resultado.Agendamento.DataHoraFinal,
                Sala = resultado.Agendamento.Sala,
                Confirmacao = resultado.Agendamento.Confirmacao
            };
    }
}
