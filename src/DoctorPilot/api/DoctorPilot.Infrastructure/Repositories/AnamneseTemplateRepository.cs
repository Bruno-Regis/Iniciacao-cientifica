using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class AnamneseTemplateRepository : Repository<AnamneseTemplate>, IAnamneseTemplateRepository
    {
        public AnamneseTemplateRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AnamneseTemplate>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(t => t.ClinicaId == clinicaId)
                .OrderBy(t => t.Nome).Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<AnamneseTemplate?> GetByIdComSessoesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(t => t.Sessoes)
                    .ThenInclude(s => s.Opcoes)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
    }
}
