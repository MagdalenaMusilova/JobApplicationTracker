using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IResumeMergeService
{
    public Task<UserResumeDto> MergeAsync(UserResumeDto existing, UserResumeDto incoming);
}