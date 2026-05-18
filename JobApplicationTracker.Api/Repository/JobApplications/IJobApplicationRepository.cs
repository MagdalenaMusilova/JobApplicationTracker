using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IJobApplicationRepository
{
    Task<IEnumerable<JobApplication>> GetAllAsync();
    Task<IEnumerable<JobApplication>> GetAllByUserAsync(string userId);
    Task<IEnumerable<JobApplicationMinimal>> GetAllByUserMinimalAsync(string userId);
    Task<IEnumerable<JobApplication>> GetAllNotFinishedAsync(string userId);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> AddAsync(JobApplication application);
    Task<JobApplication> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(Guid id);
}