using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IResumeService
{
    public Task<UserResumeDto> CreateAsync(UserResumeDto resume);
    public Task<UserResumeDto> ExtractFromPdfAsync(IFormFile file);
    public Task<UserResumeDto?> GetByIdAsync(Guid id);
    public Task<UserResumeDto?> GetByUserAsync(string userId);
    public Task<UserResumeDto?> UpdateAsync(Guid id, UserResumeDto updated);
    public Task<UserResumeDto?> MergeAsync(Guid id, UserResumeDto newResume);
    public Task<bool> DeleteAsync(Guid id);
}