using JobApplicationTracker.Models.Jobs;

namespace JobApplicationTracker.Services;

public interface IJobListingExtractor
{
    public Task<JobListingDto?> ExtractFromPlaintextAsync(string text);
}