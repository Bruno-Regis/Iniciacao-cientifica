using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>  
    {
        Task<bool> ExistsEmailAsync(string email);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetByClinicaIdAsync(Guid clinicaId, int page = 1, int pageSize = 20);
    }
}
