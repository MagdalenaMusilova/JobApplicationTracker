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

    private async Task<JobApplicationDto?> GetOwnedApplicationAsync(Guid applicationId, string userId)
    {
        var application = await _jobApplicationService.GetByIdAsync(applicationId);
        return application is not null && application.UserId == userId
            ? application
            : null;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllByAsync()
    {
        var applications = await _jobApplicationService.GetAllAsync();
        return Ok(applications);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAllByUserAsync()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var applications = await _jobApplicationService.GetAllByUserAsync(userId);
        return Ok(applications);
    }

    [HttpGet("minimal")]
    public async Task<ActionResult<IEnumerable<JobApplicationMinimalDto>>> GetAllByUserMinimalAsync()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        bool archived = Request.Query.ContainsKey("archived");
        
        var applications = await _jobApplicationService.GetAllByUserMinimalAsync(userId, archived);
        return Ok(applications);
    }

    [HttpGet("notFinished")]
    public async Task<ActionResult<IEnumerable<JobApplicationMinimalDto>>> GetAllNotFinishedAsync()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var applications = await _jobApplicationService.GetAllNotFinishedAsync(userId);
        return Ok(applications);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<JobApplicationDto>> GetById(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var application = await GetOwnedApplicationAsync(id, userId);

        if (application is null)
            return NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."));

        return Ok(application);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create([FromBody] CreateJobApplicationDto application)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        try
        {
            var created = await _jobApplicationService.AddAsync(userId, application);
            return Ok(created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto("APPLICATION_CREATE_FAILED", ex.Message));
        }
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<JobApplicationDto>> Update(Guid id, [FromBody] UpdateJobApplicationDto application)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existing = await GetOwnedApplicationAsync(id, userId);

        if (existing is null)
            return NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."));

        var updated = await _jobApplicationService.UpdateAsync(id, application);

        return updated is null
            ? NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."))
            : Ok(updated);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existing = await GetOwnedApplicationAsync(id, userId);

        if (existing is null)
            return NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."));

        var deleted = await _jobApplicationService.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."));
    }

    [HttpPut("{id:Guid}/deny")]
    public async Task<ActionResult<JobApplicationDto>> MarkApplicationAsRejected(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existing = await GetOwnedApplicationAsync(id, userId);

        if (existing is null)
            return NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."));

        var newStatus = new CreateJAStatusEntryDto
        {
            JobApplicationId = id,
            StatusType = (int)JAStatusType.Rejected,
            Note = ""
        };

        try
        {
            var updated = await _jobApplicationService.PushApplicationStatusAsync(newStatus);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto("STATUS_UPDATE_FAILED", ex.Message));
        }
    }

    [HttpPost("entry")]
    public async Task<ActionResult<JobApplicationDto>> PushApplicationStatus([FromBody] CreateJAStatusEntryDto statusEntry)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existing = await GetOwnedApplicationAsync(statusEntry.JobApplicationId, userId);

        if (existing is null)
            return NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."));

        try
        {
            var application = await _jobApplicationService.PushApplicationStatusAsync(statusEntry);
            return Ok(application);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto("STATUS_CREATE_FAILED", ex.Message));
        }
    }

    [HttpPut("entry/{entryId:Guid}")]
    public async Task<ActionResult<JAStatusEntryDto>> UpdateStatusEntry(Guid entryId, [FromBody] CreateJAStatusEntryDto statusEntry)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingEntry = await _jaStatusEntryService.GetByIdAsync(entryId);

        if (existingEntry is null)
            return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

        if (!await UserOwnsApplicationAsync(existingEntry.JobApplicationId, userId))
            return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

        if (statusEntry.JobApplicationId != existingEntry.JobApplicationId)
            return BadRequest(new ErrorResponseDto("APPLICATION_MISMATCH", "Status entry cannot be moved to another job application."));

        try
        {
            var updated = await _jaStatusEntryService.UpdateAsync(entryId, statusEntry);

            return updated is null
                ? NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."))
                : Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponseDto("STATUS_UPDATE_FAILED", ex.Message));
        }
    }

    [HttpDelete("entry/{entryId:Guid}")]
    public async Task<ActionResult<JobApplicationDto>> DeleteJAStatusEntry(Guid entryId)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingEntry = await _jaStatusEntryService.GetByIdAsync(entryId);

        if (existingEntry is null)
            return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

        if (!await UserOwnsApplicationAsync(existingEntry.JobApplicationId, userId))
            return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

        try
        {
            var deleted = await _jaStatusEntryService.DeleteAsync(entryId);

            if (!deleted)
                return NotFound(new ErrorResponseDto("STATUS_ENTRY_NOT_FOUND", "Status entry was not found."));

            var application = await _jobApplicationService.GetByIdAsync(existingEntry.JobApplicationId);

            return application is null
                ? NotFound(new ErrorResponseDto("APPLICATION_NOT_FOUND", "Job application was not found."))
                : Ok(application);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ErrorResponseDto("STATUS_DELETE_FAILED", ex.Message));
        }
    }
}