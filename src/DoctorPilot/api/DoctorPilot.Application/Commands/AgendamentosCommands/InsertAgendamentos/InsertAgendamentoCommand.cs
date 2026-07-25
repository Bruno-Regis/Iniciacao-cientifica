using DoctorPilot.Contracts.Common;
using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using Mediator;

namespace DoctorPilot.Application.Commands.AgendamentosCommands.InsertAgendamentos
{
    public class InsertAgendamentoCommand : ICommand<ResultViewModel<Guid>>
    {
        public Guid PacienteId { get; set; }
        public Guid ProfissionalId { get; set; }
        public Guid ClinicaId { get; set; }
        public TipoAgendamentoEnum Tipo { get; set; }
        public string Procedimento { get; set; }
        public DateTime DataHoraInicial { get; set; }
        public DateTime DataHoraFinal { get; set; }
        public string? Sala { get; set; }
        public bool Confirmacao { get; set; }
        public string? Descricao { get; set; }


        public Agendamento ToEntity()
            => new(PacienteId, ProfissionalId, ClinicaId, Tipo, Procedimento, DataHoraInicial, DataHoraFinal, Sala, Confirmacao, Descricao);
    }
}
