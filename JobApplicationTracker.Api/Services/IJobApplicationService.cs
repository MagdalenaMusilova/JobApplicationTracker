using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public interface IJobApplicationService
{
    Task<IEnumerable<JobApplicationDto>> GetAllAsync();
    Task<IEnumerable<JobApplicationDto>> GetAllByUserAsync(int userId);
    Task<JobApplicationDto?> GetByIdAsync(int id);
    Task<JobApplicationDto> AddAsync(CreateJobApplicationDto application);
    Task<JobApplicationDto?> UpdateAsync(int id, UpdateJobApplicationDto application);
    Task<bool> DeleteAsync(int id);
    Task<JobApplicationDto> PushApplicationStatusAsync(CreateJAStatusEntryDto statusEntry);
    Task<JobApplicationDto> DeleteJAStatusEntryAsync(int entryId);
}