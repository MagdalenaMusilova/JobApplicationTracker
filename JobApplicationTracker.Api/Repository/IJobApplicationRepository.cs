using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IJobApplicationRepository
{
    IQueryable<JobApplication> Query(string userId);
    Task<IEnumerable<JobApplication>> GetAllAsync();
    Task<IEnumerable<JobApplication>> GetAllNotFinishedAsync();
    Task<IEnumerable<JobApplication>> GetAllByUserAsync(string userId);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> AddAsync(JobApplication application);
    Task<JobApplication> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(Guid id);
}