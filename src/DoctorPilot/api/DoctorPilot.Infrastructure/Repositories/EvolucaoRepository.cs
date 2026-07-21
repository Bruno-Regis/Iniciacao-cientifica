using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class EvolucaoRepository : Repository<Evolucao>, IEvolucaoRepository
    {
        public EvolucaoRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<Evolucao?> GetByAgendamentoIdAsync(Guid agendamentoId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(e => e.AgendamentoId == agendamentoId);
        }

        public async Task<IEnumerable<Evolucao>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                 .Where(e => _context.Agendamentos
                     .Any(a => a.Id == e.AgendamentoId && a.PacienteId == pacienteId))
                 .OrderByDescending(e => e.CreatedAt)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
        }
    }
}
