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

        public async Task<Evolucao?> GetByAgendamentoIdAsync(
            Guid agendamentoId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(e => e.Agendamento)
                .ThenInclude(a => a.Profissional)
                .FirstOrDefaultAsync(e => e.AgendamentoId == agendamentoId, cancellationToken);
        }

        public async Task<IEnumerable<Evolucao>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                 .AsNoTracking()
                 .Include(e => e.Agendamento)
                 .ThenInclude(a => a.Profissional)
                 .Where(e => _context.Agendamentos
                     .Any(a => a.Id == e.AgendamentoId && a.PacienteId == pacienteId))
                 .OrderByDescending(e => e.Agendamento.DataHoraInicial)
                 .ThenByDescending(e => e.Id)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
        }
    }
}
