using JobApplicationTracker.Dos;

namespace JobApplicationTracker.Repository;

public interface IJAStatusEntryRepository
{
    public Task<JAStatusEntryDo?> GetByIdAsync(int id);
    Task<JAStatusEntryDo> AddAsync(JAStatusEntryDo jaStatusEntry);
    Task<bool> DeleteBulkAsync(IEnumerable<int> ids);
}