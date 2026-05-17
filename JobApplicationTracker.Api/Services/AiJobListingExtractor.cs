using System.Text.Json;
using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public class AiJobListingExtractor : IJobListingExtractor
{
    private readonly IAiAgentService _aiAgentService;

    public AiJobListingExtractor(IAiAgentService aiAgentService)
    {
        _aiAgentService = aiAgentService;
    }
    
    public Task<JobListingDto?> ExtractFromPlaintextAsync(string text)
    {
        JobListingDto res = new JobListingDto()
        {
            JobDescription = text
        };
        return Task.FromResult(res);
    }
}