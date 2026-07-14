using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat?> GetByIdComMensagensAsync(Guid id);
        Task<IEnumerable<Chat>> GetByUsuarioIdAsync(Guid usuarioId, int page = 1, int pageSize = 20);
    }
}
