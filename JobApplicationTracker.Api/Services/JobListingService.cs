using JobApplicationTracker.Models.Jobs;

namespace JobApplicationTracker.Services;

public class JobListingService: IJobListingService
{
    private readonly IJobListingExtractor _jobListingExtractor;
    
    public JobListingService(IJobListingExtractor jobListingExtractor)
    {
        _jobListingExtractor = jobListingExtractor;
    }
    
    public Task<JobListingDto?> ExtractFromPlaintext(string text)
    {   
        var res = _jobListingExtractor.ExtractFromPlaintextAsync(text);
        return res;
    }
}