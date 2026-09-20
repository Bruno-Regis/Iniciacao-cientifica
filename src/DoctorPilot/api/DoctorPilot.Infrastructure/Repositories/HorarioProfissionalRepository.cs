using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class HorarioProfissionalRepository : Repository<HorarioProfissional>, IHorarioProfissionalRepository
    {
        public HorarioProfissionalRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsConflitoAsync(Guid profissionalId, DayOfWeek diaSemana, TimeOnly horaInicio, TimeOnly horaFim, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(
                    h => h.ProfissionalId == profissionalId
                         && h.DiaSemana == diaSemana
                         && h.HoraInicio < horaFim
                         && h.HoraFim > horaInicio,
                         cancellationToken);
        }

        public async Task<IEnumerable<HorarioProfissional>> GetByProfissionalIdAsync(Guid profissionalId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(h => h.ProfissionalId == profissionalId)
                .OrderBy(h => h.DiaSemana)
                .ThenBy(h => h.HoraInicio)
                .ToListAsync(cancellationToken);
        }

    }
}
