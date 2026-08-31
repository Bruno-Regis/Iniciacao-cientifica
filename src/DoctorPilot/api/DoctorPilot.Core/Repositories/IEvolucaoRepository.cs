using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IEvolucaoRepository : IRepository<Evolucao>
    {
        Task<IEnumerable<Evolucao>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20);
        Task<Evolucao?> GetByAgendamentoIdAsync(Guid agendamentoId, CancellationToken cancellationToken = default);

    }
}
