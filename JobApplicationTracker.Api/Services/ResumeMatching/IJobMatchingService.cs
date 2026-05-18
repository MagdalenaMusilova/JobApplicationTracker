using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IJobMatchingService
{
    public Task<string> EvaluateMatch(UserResumeDto resume, string jobListing);
}