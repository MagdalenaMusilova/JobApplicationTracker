using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Services;

public interface IJAStatusEntryService
{
    public Task<JAStatusEntryDto?> GetByIdAsync(Guid id);
    public Task<List<JAStatusEntryDto>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationIds);
    public Task<JAStatusEntryDto> AddAsync(JobApplicationDto jobApplication, CreateJAStatusEntryDto jaStatusEntry);
    Task<JAStatusEntryDto?> UpdateAsync(Guid id, CreateJAStatusEntryDto updated);
    Task<bool> DeleteAsync(Guid id);
}