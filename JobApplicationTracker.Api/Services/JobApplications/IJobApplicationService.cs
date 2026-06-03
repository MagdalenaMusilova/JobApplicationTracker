using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IJobApplicationService
{
    Task<IEnumerable<JobApplicationDto>> GetAllAsync();
    Task<IEnumerable<JobApplicationDto>> GetAllByUserAsync(string userId);
    Task<IEnumerable<JobApplicationMinimalDto>> GetAllByUserMinimalAsync(string userId);
    Task<IEnumerable<JobApplicationMinimalDto>> GetAllNotFinishedAsync(string userId);
    Task<JobApplicationDto?> GetByIdAsync(Guid id);
    Task<JobApplicationDto> AddAsync(string userId, CreateJobApplicationDto application);
    Task<JobApplicationDto?> UpdateAsync(Guid id, UpdateJobApplicationDto application);
    Task<bool> DeleteAsync(Guid id);
    Task<JobApplicationDto> PushApplicationStatusAsync(CreateJAStatusEntryDto statusEntry);
}