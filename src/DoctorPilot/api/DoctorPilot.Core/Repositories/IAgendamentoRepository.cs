using DoctorPilot.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorPilot.Core.Repositories
{
    public interface IAgendamentoRepository : IRepository<Agendamento>
    {
        Task<IEnumerable<Agendamento>> GetByPeriodoAsync(Guid clinicaId, DateTime inicio, DateTime fim, Guid? profissionalId = null);
        Task<IEnumerable<Agendamento>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20);
        Task<Agendamento?> GetUltimoAgendamentoPorPacienteAsync(Guid pacienteId);
    }
}
