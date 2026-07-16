using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        Task<Paciente?> GetByIdComAlergias(Guid id);
        Task<Paciente?> GetByCpfAsync(string cpf);
        Task<IEnumerable<Paciente>> SearchAsync(string termo, int page = 1, int pageSize = 20);
    }
}
