using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IResumeService
{
    public Task<UserResumeDto> ExtractFromPdf(IFormFile file);
}