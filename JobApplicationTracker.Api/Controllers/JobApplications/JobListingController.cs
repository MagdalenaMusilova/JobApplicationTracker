using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/jobListings")]
[Authorize]
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
        if (string.IsNullOrWhiteSpace(data.JobListing))
            return BadRequest(new ErrorResponseDto("JOB_LISTING_REQUIRED", "Job listing text is required."));

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var resume = await _resumeService.GetByUserAsync(userId);

        var res = await _jobMatchingService.EvaluateMatch(resume, data.JobListing);

        return Ok(res);
    }
}