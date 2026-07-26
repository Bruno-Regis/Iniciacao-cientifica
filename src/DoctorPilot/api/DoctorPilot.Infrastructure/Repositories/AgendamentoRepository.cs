using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class AgendamentoRepository : Repository<Agendamento>, IAgendamentoRepository
    {
        public AgendamentoRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Agendamento>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Where(a => a.PacienteId == pacienteId)
                .OrderByDescending(a => a.DataHoraInicial)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<IEnumerable<Agendamento>> GetByPeriodoAsync(Guid clinicaId, DateTime inicio, DateTime fim, Guid? profissionalId = null)
        {
            var query = _dbSet
                 .Where(a => a.ClinicaId == clinicaId
                          && a.DataHoraInicial >= inicio
                          && a.DataHoraFinal <= fim);

            if (profissionalId.HasValue)
                query = query.Where(a => a.ProfissionalId == profissionalId.Value);

            return await query
                .OrderBy(a => a.DataHoraInicial)
                .ToListAsync();
        }

        public async Task<Agendamento?> GetUltimoAgendamentoPorPacienteAsync(Guid pacienteId)
        {
            return await _dbSet
                .Where(a => a.PacienteId == pacienteId)
                .OrderByDescending(a => a.DataHoraInicial)
                .FirstOrDefaultAsync();
        }
    }
}
