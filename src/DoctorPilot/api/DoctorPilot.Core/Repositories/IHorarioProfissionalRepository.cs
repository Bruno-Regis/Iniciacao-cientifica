
using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IHorarioProfissionalRepository : IRepository<HorarioProfissional>
    {
        Task<IEnumerable<HorarioProfissional>> GetByProfissionalIdAsync(Guid profissionalId, CancellationToken cancellationToken = default);
        Task<bool> ExistsConflitoAsync(Guid profissionalId,DayOfWeek diaSemana, TimeOnly horaInicio, TimeOnly horaFim, CancellationToken cancellationToken = default);
    }
}
