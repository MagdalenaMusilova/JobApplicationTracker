using JobApplicationTracker.Dos;

namespace JobApplicationTracker.Repository;

public interface IJobApplicationRepository
{
    Task<IEnumerable<JobApplicationDo>> GetAllAsync();
    Task<IEnumerable<JobApplicationDo>> GetAllByUserAsync(Guid userId);
    Task<JobApplicationDo?> GetByIdAsync(Guid id);
    Task<JobApplicationDo> AddAsync(JobApplicationDo application);
    Task<JobApplicationDo> UpdateAsync(JobApplicationDo application);
    Task<bool> DeleteAsync(Guid id);
}