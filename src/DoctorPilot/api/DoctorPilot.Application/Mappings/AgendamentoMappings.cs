using DoctorPilot.Contracts.Agendamentos;
using DoctorPilot.Core.Entities;

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
                Status = agendamento.Status.ToString(),
                Tipo = agendamento.Tipo.ToString(),
                Procedimento = agendamento.Procedimento,
                DataHoraInicial = agendamento.DataHoraInicial,
                DataHoraFinal = agendamento.DataHoraFinal,
                Sala = agendamento.Sala,
                Confirmacao = agendamento.Confirmacao
            };
    }
}
