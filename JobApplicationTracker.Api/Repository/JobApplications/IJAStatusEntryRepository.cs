using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IJAStatusEntryRepository
{
    public Task<JAStatusEntry?> GetByIdAsync(Guid id);
    public Task<IEnumerable<JAStatusEntry>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationIds);
    Task<JAStatusEntry> AddAsync(JAStatusEntry jaStatusEntry);
    Task<JAStatusEntry?> UpdateAsync(Guid id, JAStatusEntry updated);
    Task<bool> DeleteAsync(Guid id);
}