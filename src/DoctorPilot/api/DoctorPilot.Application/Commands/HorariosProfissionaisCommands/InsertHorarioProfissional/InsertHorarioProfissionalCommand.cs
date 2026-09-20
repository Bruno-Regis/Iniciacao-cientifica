
using DoctorPilot.Contracts.Common;
using Mediator;

namespace DoctorPilot.Application.Commands.HorariosProfissionaisCommands.InsertHorarioProfissional
{
    public class InsertHorarioProfissionalCommand : ICommand<ResultViewModel<Guid>>
    {
        public InsertHorarioProfissionalCommand(Guid profissionalId, DayOfWeek diaSemana, TimeOnly horaInicio, TimeOnly horaFim)
        {
            ProfissionalId = profissionalId;
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFim = horaFim;
        }

        public Guid ProfissionalId { get; }
        public DayOfWeek DiaSemana { get; }
        public TimeOnly HoraInicio { get; }
        public TimeOnly HoraFim { get; }
    }
}
