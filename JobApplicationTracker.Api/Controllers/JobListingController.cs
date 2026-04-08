using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/jobListings")]
public class JobListingController : ControllerBase
{
    private readonly IResumeService _resumeService;
    private readonly IJobMatchingService _jobMatchingService;
    
    public JobListingController(IResumeService resumeService, IJobMatchingService jobMatchingService)
    {
        _resumeService = resumeService;
        _jobMatchingService = jobMatchingService;
    }

    
    [HttpPost("match")]
    public async Task<ActionResult<string>> Create([FromForm] MatchRequestDto data)
    {
        var resumeDto = await _resumeService.ExtractFromPdf(data.ResumeFile);
        var res = await _jobMatchingService.EvaluateMatch(resumeDto, data.JobListing);
        
        return Ok(res);
    }
}