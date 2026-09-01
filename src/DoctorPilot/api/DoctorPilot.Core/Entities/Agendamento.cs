using DoctorPilot.Core.Enums;

namespace DoctorPilot.Core.Entities
{
    public class Agendamento : BaseEntity
    {
        public Agendamento(Guid pacienteId, Guid profissionalId, Guid clinicaId, TipoAgendamentoEnum tipo, string procedimento, DateTime dataHoraInicial, DateTime dataHoraFinal, string? sala, bool confirmacao, string? descricao)
        {
            PacienteId = pacienteId;
            ProfissionalId = profissionalId;
            ClinicaId = clinicaId;
            Status = StatusAgendamentoEnum.agendado;
            Tipo = tipo;
            Procedimento = procedimento;
            DataHoraInicial = dataHoraInicial;
            DataHoraFinal = dataHoraFinal;
            Sala = sala;
            Confirmacao = confirmacao;
            Descricao = descricao;
        }

        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public Profissional Profissional { get; set; } = null!;
        public Guid ClinicaId { get; set; }
        public StatusAgendamentoEnum Status { get; set; }
        public TipoAgendamentoEnum Tipo { get; set; }
        public string Procedimento { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string? Sala { get; set; }
        public bool Confirmacao { get; set; }
        public string? Descricao { get; set; }

        public void Cancelar()
        {
            Status = StatusAgendamentoEnum.cancelado;
        }

        public void Confirmar()
        {
            Confirmacao = true;
        }

        public void Atualizar(Guid pacienteId, Guid profissionalId, TipoAgendamentoEnum tipo, string procedimento, DateTime dataHoraInicial, DateTime dataHoraFinal, string? sala, string? descricao)
        {
            PacienteId = pacienteId;
            ProfissionalId = profissionalId;
            Tipo = tipo;
            Procedimento = procedimento;
            DataHoraInicial = dataHoraInicial;
            DataHoraFinal = dataHoraFinal;
            Sala = sala;
            Descricao = descricao;
        }
    }
}
