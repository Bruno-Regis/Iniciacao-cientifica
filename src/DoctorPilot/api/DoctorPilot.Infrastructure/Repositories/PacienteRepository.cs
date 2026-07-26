using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class PacienteRepository : Repository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Paciente>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Where(p => p.ClinicaId == clinicaId)
                .OrderBy(p => p.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Paciente?> GetByCpfAsync(string cpf)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Cpf == cpf);
        }

        public async Task<Paciente?> GetByIdComAlergias(Guid id)
        {
            return await _dbSet
                .Include(p => p.Alergias)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Paciente>> SearchAsync(Guid clinicaId, string termo, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Where(p => p.ClinicaId == clinicaId)
                .Where(p => p.Nome.Contains(termo)
                         || p.Email.Contains(termo)
                         || p.Celular.Contains(termo)
                         || p.Cpf.Contains(termo))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
