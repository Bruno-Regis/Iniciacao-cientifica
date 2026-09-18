using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class ProfissionalRepository : Repository<Profissional>, IProfissionalRepository
    {
        public ProfissionalRepository(DoctorPilotDbContext context) : base(context) { }

        public async Task<Profissional?> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Clinica)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId, cancellationToken);
        }

        public async Task<Profissional?> GetByIdComRelacionamentosAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Profissional>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.ClinicaId == clinicaId)
                .OrderBy(p => p.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Profissional>> SearchByNomeAsync(Guid clinicaId, string nome, int limit = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p =>
                    p.ClinicaId == clinicaId &&
                    p.Nome.Contains(nome))
                .OrderBy(p => p.Nome)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }
    }
}
