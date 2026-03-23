using JobApplicationTracker.Dos;

namespace JobApplicationTracker.Repository;

public interface IJAStatusEntryRepository
{
    Task<JAStatusEntryDo> AddAsync(JAStatusEntryDo jaStatusEntry);
    Task<bool> DeleteBulkAsync(IEnumerable<int> ids);
}