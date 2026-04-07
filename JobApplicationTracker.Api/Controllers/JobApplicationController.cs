using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/applications")]
public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService; 
    
    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllAsync()
    {
        var applications = await _jobApplicationService.GetAllAsync();
        return Ok(applications);
    }
    
    [HttpGet("user={userId:int}")]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllByUserIdAsync(int userId)
    {
        var applications = await _jobApplicationService.GetAllByUserAsync(userId);
        return Ok(applications);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<JobApplicationDto>> GetById(int id)
    {
        var application = await _jobApplicationService.GetByIdAsync(id);

        if (application is null)
        {
            return NotFound();
        }

        return Ok(application);
    }
    
    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create([FromBody]CreateJobApplicationDto application)
    {
        var createdApplication = await _jobApplicationService.AddAsync(application);
        return Ok(createdApplication);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<JobApplicationDto>> Update(int id, [FromBody] UpdateJobApplicationDto application)
    {
        var updatedApplication = await _jobApplicationService.UpdateAsync(id, application);

        if (updatedApplication is null)
        {
            return NotFound();
        }

        return Ok(updatedApplication);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _jobApplicationService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("entry")]
    public async Task<ActionResult<JobApplicationDto>> PushApplicationStatus([FromBody] CreateJAStatusEntryDto statusEntry)
    {
        var application = await _jobApplicationService.PushApplicationStatusAsync(statusEntry);
        return Ok(application);
    }
    
    [HttpDelete("entry/{entryId:int}")]
    public async Task<ActionResult<JobApplicationDto>> DeleteJAStatusEntry(int entryId)
    {
        var application = await _jobApplicationService.DeleteJAStatusEntryAsync(entryId);
        return Ok(application);
    }
}