using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Enums;
using DoctorPilot.Core.ReadModels;
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

        public async Task<IEnumerable<AgendamentoComNomes>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var agendamentos = _dbSet
                .AsNoTracking()
                .Where(a => a.PacienteId == pacienteId)
                .OrderByDescending(a => a.DataHoraInicial)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await GetAgendamentosComNomes(agendamentos)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AgendamentoComNomes>> GetElegiveisParaEvolucaoByPacienteIdAsync(
            Guid pacienteId,
            Guid? clinicaId,
            CancellationToken cancellationToken = default)
        {
            var agendamentos = _dbSet
                .AsNoTracking()
                .Where(a => a.PacienteId == pacienteId
                         && (!clinicaId.HasValue || a.ClinicaId == clinicaId.Value)
                         && a.Status != StatusAgendamentoEnum.cancelado
                         && !_context.Evolucoes.Any(e => e.AgendamentoId == a.Id))
                .OrderByDescending(a => a.DataHoraInicial)
                .ThenByDescending(a => a.Id);

            return await GetAgendamentosComNomes(agendamentos)
                .ToListAsync(cancellationToken);
        }


        public async Task<IEnumerable<AgendamentoComNomes>> GetByPeriodoAsync(Guid clinicaId, DateTime inicio, DateTime fim, Guid? profissionalId = null, CancellationToken cancellationToken = default)
        {
            var agendamentos = _dbSet
                .AsNoTracking()
                .Where(a => a.ClinicaId == clinicaId
                         && a.DataHoraInicial < fim
                         && a.DataHoraFinal > inicio);

            if (profissionalId.HasValue)
                agendamentos = agendamentos.Where(a => a.ProfissionalId == profissionalId.Value);

            agendamentos = agendamentos.OrderBy(a => a.DataHoraInicial);

            return await GetAgendamentosComNomes(agendamentos)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExisteConflitoProfissionalAsync(Guid clinicaId, Guid profissionalId, DateTime inicio, DateTime fim, Guid? agendamentoIdIgnorado = null, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(a => a.ClinicaId == clinicaId
                            && a.ProfissionalId == profissionalId
                            && (!agendamentoIdIgnorado.HasValue || a.Id != agendamentoIdIgnorado.Value)
                            && a.Status != StatusAgendamentoEnum.cancelado
                            && a.DataHoraInicial < fim
                            && a.DataHoraFinal > inicio,
                    cancellationToken);
        }

        public async Task<bool> ExisteConflitoSalaAsync(Guid clinicaId, string sala, DateTime inicio, DateTime fim, Guid? agendamentoIdIgnorado = null, CancellationToken cancellationToken = default)
        {
            var salaNormalizada = sala.Trim().ToUpper();

            return await _dbSet
                .AsNoTracking()
                .AnyAsync(a => a.ClinicaId == clinicaId
                            && a.Status != StatusAgendamentoEnum.cancelado
                            && (!agendamentoIdIgnorado.HasValue || a.Id != agendamentoIdIgnorado.Value)
                            && a.Sala != null
                            && a.Sala.Trim().ToUpper() == salaNormalizada
                            && a.DataHoraInicial < fim
                            && a.DataHoraFinal > inicio,
                    cancellationToken);
        }

        public async Task<Agendamento?> GetUltimoAgendamentoPorPacienteAsync(Guid pacienteId)
        {
            return await _dbSet
                .Where(a => a.PacienteId == pacienteId)
                .OrderByDescending(a => a.DataHoraInicial)
                .FirstOrDefaultAsync();
        }

        private IQueryable<AgendamentoComNomes> GetAgendamentosComNomes(IQueryable<Agendamento> agendamentos)
        {
            return from agendamento in agendamentos
                   join paciente in _context.Pacientes.AsNoTracking()
                       on new { Id = agendamento.PacienteId, agendamento.ClinicaId }
                       equals new { paciente.Id, paciente.ClinicaId }
                   join profissional in _context.Profissionais.AsNoTracking()
                       on new { Id = agendamento.ProfissionalId, agendamento.ClinicaId }
                       equals new { profissional.Id, profissional.ClinicaId }
                   select new AgendamentoComNomes(agendamento, paciente.Nome, profissional.Nome);
        }
    }
}
