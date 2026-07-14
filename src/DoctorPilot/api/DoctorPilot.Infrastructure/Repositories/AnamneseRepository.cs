using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class AnamneseRepository : Repository<Anamnese>, IAnamneseRepository
    {
        public AnamneseRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<Anamnese?> GetByIdComSessoesAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.Sessoes)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Anamnese>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Where(a => a.PacienteId == pacienteId)
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
