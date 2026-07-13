using DoctorPilot.Core.Repositories;
using DoctorPilot.Infrastructure.Persistence;

namespace DoctorPilot.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DoctorPilotDbContext _context;

        public UnitOfWork(DoctorPilotDbContext context)
        {
            _context = context;
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
