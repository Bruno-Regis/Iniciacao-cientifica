using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IAnamneseRepository : IRepository<Anamnese>
    {
        Task<Anamnese?> GetByIdComSessoesAsync(Guid id);
        Task<IEnumerable<Anamnese>> GetByPacienteIdAsync(Guid pacienteId, int page = 1, int pageSize = 20);
    }
}
