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
        if (data.ResumeFile == null || data.ResumeFile.Length == 0)
            return BadRequest(new ErrorResponseDto("PDF_FILE_REQUIRED", "Please upload a PDF resume file."));

        if (Path.GetExtension(data.ResumeFile.FileName).ToLowerInvariant() != ".pdf")
            return BadRequest(new ErrorResponseDto("INVALID_FILE_TYPE", "Only PDF files are allowed."));

        if (string.IsNullOrWhiteSpace(data.JobListing))
            return BadRequest(new ErrorResponseDto("JOB_LISTING_REQUIRED", "Job listing text is required."));

        var resumeDto = await _resumeService.ExtractFromPdfAsync(data.ResumeFile);
        var res = await _jobMatchingService.EvaluateMatch(resumeDto, data.JobListing);

        return Ok(res);
    }
}