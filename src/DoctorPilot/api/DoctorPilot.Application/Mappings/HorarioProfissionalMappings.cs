
using DoctorPilot.Contracts.HorariosProfissionais;
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Application.Mappings
{
    public static class HorarioProfissionalMappings
    {
        public static HorarioProfissionalDto ToDto(
        this HorarioProfissional horario)
        => new()
        {
            Id = horario.Id,
            DiaSemana = horario.DiaSemana,
            HoraInicio = horario.HoraInicio,
            HoraFim = horario.HoraFim
        };
    }
}
