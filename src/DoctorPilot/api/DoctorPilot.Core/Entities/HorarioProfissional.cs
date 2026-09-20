
namespace DoctorPilot.Core.Entities
{
    public class HorarioProfissional : BaseEntity
    {
        public HorarioProfissional(Guid profissionalId, DayOfWeek diaSemana, TimeOnly horaInicio, TimeOnly horaFim)
        {
            ProfissionalId = profissionalId;
            DiaSemana = diaSemana;
            HoraInicio = horaInicio;
            HoraFim = horaFim;
        }

        public Guid ProfissionalId { get; private set; }

        public DayOfWeek DiaSemana { get; private set; }

        public TimeOnly HoraInicio { get; private set; }

        public TimeOnly HoraFim { get; private set; }

        public Profissional Profissional { get; private set; } = null!;
    }
}
