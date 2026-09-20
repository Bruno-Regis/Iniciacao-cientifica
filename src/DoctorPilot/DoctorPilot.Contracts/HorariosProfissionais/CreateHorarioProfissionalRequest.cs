
namespace DoctorPilot.Contracts.HorariosProfissionais
{
    public class CreateHorarioProfissionalRequest
    {
        public Guid ProfissionalId { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
    }
}
