using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public interface IJobApplicationService
{
    Task<IEnumerable<JobApplicationDto>> GetAllByUserAsync(Guid userId);
    Task<JobApplicationDto?> GetByIdAsync(Guid id);
    Task<JobApplicationDto> AddAsync(Guid userId, CreateJobApplicationDto application);
    Task<JobApplicationDto?> UpdateAsync(Guid id, UpdateJobApplicationDto application);
    Task<bool> DeleteAsync(Guid id);
    Task<JobApplicationDto> PushApplicationStatusAsync(CreateJAStatusEntryDto statusEntry);
    Task<JobApplicationDto> DeleteJAStatusEntryAsync(Guid entryId);
}