using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/applications")]
[Authorize]
public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IJAStatusEntryService _jaStatusEntryService;
    private readonly IJAEventService _jaEventService;

    public JobApplicationController(
        IJobApplicationService jobApplicationService,
        IJAStatusEntryService jaStatusEntryService,
        IJAEventService jaEventService)
    {
        _jobApplicationService = jobApplicationService;
        _jaStatusEntryService = jaStatusEntryService;
        _jaEventService = jaEventService;
    }

    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? throw new UnauthorizedAccessException("User ID not found in token.");

    [HttpGet("/all")]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllByAsync()
    {
        var applications = await _jobApplicationService.GetAllAsync();
        return Ok(applications);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllByUserAsync()
    {
        var userId = GetUserId();
        var applications = await _jobApplicationService.GetAllByUserAsync(userId);
        return Ok(applications);
    }
    
    [HttpGet("/minimal")]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllByUserMinimalAsync()
    {
        var userId = GetUserId();
        var applications = await _jobApplicationService.GetAllByUserMinimalAsync(userId);
        return Ok(applications);
    }
    
    [HttpGet("/notFinished")]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllNotFinishedAsync()
    {
        var userId = GetUserId();
        var applications = await _jobApplicationService.GetAllNotFinishedAsync(userId);
        return Ok(applications);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<JobApplicationDto>> GetById(Guid id)
    {
        var userId = GetUserId();
        var application = await _jobApplicationService.GetByIdAsync(id);

        if (application is null || application.UserId != userId)
            return NotFound();

        return Ok(application);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create([FromBody] CreateJobApplicationDto application)
    {
        var userId = GetUserId();
        var created = await _jobApplicationService.AddAsync(userId, application);
        return Ok(created);
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<JobApplicationDto>> Update(Guid id, [FromBody] UpdateJobApplicationDto application)
    {
        var userId = GetUserId();
        var existing = await _jobApplicationService.GetByIdAsync(id);

        if (existing is null || existing.UserId != userId)
            return NotFound();

        var updated = await _jobApplicationService.UpdateAsync(id, application);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        var existing = await _jobApplicationService.GetByIdAsync(id);

        if (existing is null || existing.UserId != userId)
            return NotFound();

        var deleted = await _jobApplicationService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    
    [HttpPut("{id:Guid}/deny")]
    public async Task<ActionResult<JobApplicationDto>> MarkApplicationAsRejected(Guid id)
    {
        var userId = GetUserId();
        var existing = await _jobApplicationService.GetByIdAsync(id);

        if (existing is null || existing.UserId != userId)
            return NotFound();

        CreateJAStatusEntryDto newStatus = new CreateJAStatusEntryDto()
        {
            JobApplicationId = id,
            StatusType = (int)JAStatusType.Rejected,
            Note = ""
        };

        var updated = await _jobApplicationService.PushApplicationStatusAsync(newStatus);
        return Ok(updated);
    }

    [HttpPost("entry")]
    public async Task<ActionResult<JobApplicationDto>> PushApplicationStatus([FromBody] CreateJAStatusEntryDto statusEntry)
    {
        var userId = GetUserId();
        var existing = await _jobApplicationService.GetByIdAsync(statusEntry.JobApplicationId);

        if (existing is null || existing.UserId != userId)
            return NotFound();

        var application = await _jobApplicationService.PushApplicationStatusAsync(statusEntry);
        return Ok(application);
    }

    [HttpPut("entry/{entryId:Guid}")]
    public async Task<ActionResult<JAStatusEntryDto>> UpdateStatusEntry(Guid entryId, [FromBody] CreateJAStatusEntryDto statusEntry)
    {
        var updated = await _jaStatusEntryService.UpdateAsync(entryId, statusEntry);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("entry/{entryId:Guid}")]
    public async Task<ActionResult<JobApplicationDto>> DeleteJAStatusEntry(Guid entryId)
    {
        try
        {
            var application = await _jaStatusEntryService.DeleteBulkAsync([entryId]);
            return Ok(application);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    [HttpPost("event")]
    public async Task<ActionResult<JAEventDto>> CreateEvent([FromBody] CreateJAEventDto jaEvent)
    {
        var createdEvent = await _jaEventService.AddAsync(jaEvent);
        return Ok(createdEvent);
    }

    [HttpPut("event/{eventId:guid}")]
    public async Task<ActionResult<JAEventDto>> UpdateEvent(Guid eventId, [FromBody] UpdateJAEventDto jaEvent)
    {
        var updatedEvent = await _jaEventService.UpdateAsync(eventId, jaEvent);
        return updatedEvent is null ? NotFound() : Ok(updatedEvent);
    }

    [HttpDelete("event/{eventId:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid eventId)
    {
        var existing = await _jaEventService.GetByIdAsync(eventId);

        if (existing is null)
            return NotFound();

        var deleted = await _jaEventService.DeleteBulkAsync([eventId]);
        return deleted ? NoContent() : NotFound();
    }
    
    [HttpGet("/{userId}/events")]
    public async Task<ActionResult<IEnumerable<JAEventDto>>> GetAllUserEvents(string userId)
    {
        var events = await _jaEventService.GetAllByUserId(userId);
        return Ok(events);
    }
}