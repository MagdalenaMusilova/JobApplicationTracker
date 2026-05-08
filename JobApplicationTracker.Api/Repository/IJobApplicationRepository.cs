using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IJobApplicationRepository
{
    IQueryable<JobApplication> Query(Guid userId);
    Task<IEnumerable<JobApplication>> GetAllAsync();
    Task<IEnumerable<JobApplication>> GetAllByUserAsync(Guid userId);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> AddAsync(JobApplication application);
    Task<JobApplication> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(Guid id);
}