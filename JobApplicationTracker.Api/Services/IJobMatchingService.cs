using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models.Jobs;

namespace JobApplicationTracker.Services;

public interface IJobMatchingService
{
    public Task<string> EvaluateMatch(UserResumeDto resume, string jobListing);
}