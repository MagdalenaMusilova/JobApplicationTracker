using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IJAEventRepository
{
    Task<JAEvent?> GetByIdAsync(Guid id);
    Task<List<JAEvent>> GetByStatusIdsAsync(IEnumerable<Guid> statusIds);
    Task<List<JAEvent>> GetByUserIdAsync(string userId);
    Task<JAEvent> AddAsync(JAEvent jaEvent);
    Task<JAEvent?> UpdateAsync(JAEvent updated);
    Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids);
}