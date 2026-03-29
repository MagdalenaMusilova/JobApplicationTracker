using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Services;

public interface IJAStatusEntryService
{
    public Task<JAStatusEntryDto?> GetByIdAsync(int id);
    public Task<JAStatusEntryDto> AddAsync(JobApplicationDto jobApplication, CreateJAStatusEntryDto jaStatusEntry);
    Task<bool> DeleteBulkAsync(IEnumerable<int> ids);
}