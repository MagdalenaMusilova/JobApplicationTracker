using JobApplicationTracker.Dos;

namespace JobApplicationTracker.Repository;

public interface IJobApplicationRepository
{
    Task<IEnumerable<JobApplicationDo>> GetAllAsync();
    Task<IEnumerable<JobApplicationDo>> GetAllByUserAsync(int userId);
    Task<JobApplicationDo?> GetByIdAsync(int id);
    Task<JobApplicationDo> AddAsync(JobApplicationDo application);
    Task<JobApplicationDo> UpdateAsync(JobApplicationDo application);
    Task<bool> DeleteAsync(int id);
}