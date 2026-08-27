using DoctorPilot.Core.Entities;
using DoctorPilot.Core.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Repositories
{
    public interface IAgendamentoRepository : IRepository<Agendamento>
    {
        Task<IEnumerable<AgendamentoComNomes>> GetByPeriodoAsync(Guid clinicaId, DateTime inicio, DateTime fim, Guid? profissionalId = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<AgendamentoComNomes>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
        Task<bool> ExisteConflitoProfissionalAsync(Guid clinicaId, Guid profissionalId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
        Task<bool> ExisteConflitoSalaAsync(Guid clinicaId, string sala, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
        Task<Agendamento?> GetUltimoAgendamentoPorPacienteAsync(Guid pacienteId);
    }
}
