using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class JAEventController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IJAStatusEntryService _jaStatusEntryService;
    private readonly IJAEventService _jaEventService;

    public JAEventController(
        IJobApplicationService jobApplicationService,
        IJAStatusEntryService jaStatusEntryService,
        IJAEventService jaEventService)
    {
        _jobApplicationService = jobApplicationService;
        _jaStatusEntryService = jaStatusEntryService;
        _jaEventService = jaEventService;
    }
    
    private bool TryGetUserId(out string userId)
    {
        userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        return !string.IsNullOrWhiteSpace(userId);
    }
    
    private async Task<bool> UserOwnsApplicationAsync(Guid applicationId, string userId)
    {
        var application = await _jobApplicationService.GetByIdAsync(applicationId);
        return application is not null && application.UserId == userId;
    }
    
    [HttpPost]
    public async Task<ActionResult<JAEventDto>> CreateEvent([FromBody] CreateJAEventDto jaEvent)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var statusEntry = await _jaStatusEntryService.GetByIdAsync(jaEvent.JAStatusEntryId);

        if (statusEntry is null)
            return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

        if (!await UserOwnsApplicationAsync(statusEntry.JobApplicationId, userId))
            return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

        try
        {
            var createdEvent = await _jaEventService.AddAsync(jaEvent);
            return Ok(createdEvent);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ErrorResponseDto("EVENT_CREATE_FAILED", ex.Message));
        }
    }

    [HttpPut("{eventId:guid}")]
    public async Task<ActionResult<JAEventDto>> UpdateEvent(Guid eventId, [FromBody] UpdateJAEventDto jaEvent)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingEvent = await _jaEventService.GetByIdAsync(eventId);

        if (existingEvent is null)
            return NotFound(new ErrorResponseDto("EVENT_NOT_FOUND", "Event was not found."));

        var statusEntry = await _jaStatusEntryService.GetByIdAsync(existingEvent.JAStatusEntryId);

        if (statusEntry is null || !await UserOwnsApplicationAsync(statusEntry.JobApplicationId, userId))
            return NotFound(new ErrorResponseDto("EVENT_NOT_FOUND", "Event was not found."));

        var updatedEvent = await _jaEventService.UpdateAsync(eventId, jaEvent);

        return updatedEvent is null
            ? NotFound(new ErrorResponseDto("EVENT_NOT_FOUND", "Event was not found."))
            : Ok(updatedEvent);
    }

    [HttpDelete("{eventId:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid eventId)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingEvent = await _jaEventService.GetByIdAsync(eventId);

        if (existingEvent is null)
            return NotFound(new ErrorResponseDto("EVENT_NOT_FOUND", "Event was not found."));

        var statusEntry = await _jaStatusEntryService.GetByIdAsync(existingEvent.JAStatusEntryId);

        if (statusEntry is null || !await UserOwnsApplicationAsync(statusEntry.JobApplicationId, userId))
            return NotFound(new ErrorResponseDto("EVENT_NOT_FOUND", "Event was not found."));

        var deleted = await _jaEventService.DeleteBulkAsync([eventId]);

        return deleted
            ? NoContent()
            : NotFound(new ErrorResponseDto("EVENT_NOT_FOUND", "Event was not found."));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JAEventDto>>> GetAllUserEvents()
    {
        if (!TryGetUserId(out var currentUserId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        if (string.IsNullOrWhiteSpace(currentUserId))
            return BadRequest(new ErrorResponseDto("USER_ID_REQUIRED", "User ID is required."));

        var events = await _jaEventService.GetAllByUserId(currentUserId);
        return Ok(events);
    }
}