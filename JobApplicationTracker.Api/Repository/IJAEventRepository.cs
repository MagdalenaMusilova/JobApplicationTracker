using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IJAEventRepository
{
    Task<JAEventDo?> GetByIdAsync(Guid id);
    Task<List<JAEventDo>> GetByStatusIdsAsync(IEnumerable<Guid> statusIds);
    Task<JAEventDo> AddAsync(JAEventDo jaEvent);
    Task<JAEventDo?> UpdateAsync(JAEventDo updated);
    Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids);
}