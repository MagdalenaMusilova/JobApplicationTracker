using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Services;

public interface IJAStatusEntryService
{
    public Task<JAStatusEntryDto?> GetByIdAsync(Guid id);
    public Task<List<JAStatusEntryDto>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationId);
    public Task<JAStatusEntryDto> AddAsync(JobApplicationDto jobApplication, CreateJAStatusEntryDto jaStatusEntry);
    Task<JAStatusEntryDto?> UpdateAsync(Guid id, CreateJAStatusEntryDto updated);
    Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids);
}