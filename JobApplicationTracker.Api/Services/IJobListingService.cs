
using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IJobListingService
{
    public Task<JobListingDto?> ExtractFromPlaintext(string text);
}