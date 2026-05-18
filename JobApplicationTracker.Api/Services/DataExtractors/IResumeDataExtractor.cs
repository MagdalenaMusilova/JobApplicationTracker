
using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IResumeDataExtractor
{
    public Task<UserResumeDto?> ExtractFromPlaintextAsync(string text);
}