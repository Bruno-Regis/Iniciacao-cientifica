using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        Task<Paciente?> GetByIdComAlergias(Guid id);
        Task<Paciente?> GetByCpfAsync(string cpf);
        Task<IEnumerable<Paciente>> SearchAsync(Guid clinicaId, string termo, int page = 1, int pageSize = 20);
        Task<IEnumerable<Paciente>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20);

    }
}
