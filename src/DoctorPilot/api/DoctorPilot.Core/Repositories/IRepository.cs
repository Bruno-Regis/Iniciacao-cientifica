using DoctorPilot.Core.Entities;

namespace DoctorPilot.Core.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 20);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id); // soft delete — só seta IsDeleted = true
    }
}
