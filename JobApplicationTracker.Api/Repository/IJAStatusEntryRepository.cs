using JobApplicationTracker.Dos;

namespace JobApplicationTracker.Repository;

public interface IJAStatusEntryRepository
{
    public Task<JAStatusEntryDo?> GetByIdAsync(Guid id);
    public Task<IEnumerable<JAStatusEntryDo>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationIds);
    Task<JAStatusEntryDo> AddAsync(JAStatusEntryDo jaStatusEntry);
    Task<JAStatusEntryDo?> UpdateAsync(Guid id, JAStatusEntryDo updated);
    Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids);
}