using DoctorPilot.Core.Entities;
using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(DoctorPilotDbContext context) : base(context)
        {
        }

        public async Task<Chat?> GetByIdComMensagensAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.Mensagens)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Chat>> GetByUsuarioIdAsync(Guid usuarioId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
