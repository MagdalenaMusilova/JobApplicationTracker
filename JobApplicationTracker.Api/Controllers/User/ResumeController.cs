using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/profile/resume")]
[Authorize]
public class ResumeController : ControllerBase
{
    private readonly IResumeService _resumeService;

    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    private bool TryGetUserId(out string userId)
    {
        userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        return !string.IsNullOrWhiteSpace(userId);
    }

    [HttpGet]
    public async Task<ActionResult<UserResumeDto>> Get()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var resume = await _resumeService.GetByUserAsync(userId);

        if (resume is null)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        return Ok(resume);
    }

    [HttpPost]
    public async Task<ActionResult<UserResumeDto>> Create([FromBody] UserResumeDto resume)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        resume.UserId = userId;

        var existingResume = await _resumeService.GetByUserAsync(userId);
        if (existingResume is not null)
            return Conflict(new ErrorResponseDto("RESUME_ALREADY_EXISTS", "A resume already exists for this user."));

        var created = await _resumeService.CreateAsync(resume);
        return Ok(created);
    }

    [HttpPost("extract")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ExtractFromPdf([FromForm] PdfUploadRequestDto request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var file = request.File;

        if (file == null || file.Length == 0)
            return BadRequest(new ErrorResponseDto("PDF_FILE_REQUIRED", "Please upload a PDF file."));

        if (Path.GetExtension(file.FileName).ToLowerInvariant() != ".pdf")
            return BadRequest(new ErrorResponseDto("INVALID_FILE_TYPE", "Only PDF files are allowed."));

        var extracted = await _resumeService.ExtractFromPdfAsync(file);
        extracted.UserId = userId;

        // Delete existing resume if it exists
        var existingResume = await _resumeService.GetByUserAsync(userId);
        if (existingResume is not null)
        {
            await _resumeService.DeleteAsync(existingResume.Id);
        }

        // Create new resume with extracted data
        var created = await _resumeService.CreateAsync(extracted);
        return Ok(created);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResumeDto>> GetById(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var resume = await _resumeService.GetByIdAsync(id);

        if (resume is null || resume.UserId != userId)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        return Ok(resume);
    }

    [HttpGet("user={userId}")]
    public async Task<ActionResult<UserResumeDto>> GetByUserId(string userId)
    {
        if (!TryGetUserId(out var currentUserId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        if (userId != currentUserId)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        var resume = await _resumeService.GetByUserAsync(userId);

        if (resume is null)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        return Ok(resume);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResumeDto>> Update(Guid id, [FromBody] UserResumeDto resume)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingResume = await _resumeService.GetByIdAsync(id);

        if (existingResume is null || existingResume.UserId != userId)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        resume.Id = id;
        resume.UserId = userId;

        var updated = await _resumeService.UpdateAsync(id, resume);

        return updated is null
            ? NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."))
            : Ok(updated);
    }

    [HttpPut("{id:guid}/merge")]
    public async Task<ActionResult<UserResumeDto>> Merge(Guid id, [FromForm] PdfUploadRequestDto request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingResume = await _resumeService.GetByIdAsync(id);

        if (existingResume is null || existingResume.UserId != userId)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        var file = request.File;

        if (file == null || file.Length == 0)
            return BadRequest(new ErrorResponseDto("PDF_FILE_REQUIRED", "Please upload a PDF file."));

        if (Path.GetExtension(file.FileName).ToLowerInvariant() != ".pdf")
            return BadRequest(new ErrorResponseDto("INVALID_FILE_TYPE", "Only PDF files are allowed."));

        var extracted = await _resumeService.ExtractFromPdfAsync(file);
        var merged = await _resumeService.MergeAsync(id, extracted);

        return merged is null
            ? NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."))
            : Ok(merged);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));

        var existingResume = await _resumeService.GetByIdAsync(id);

        if (existingResume is null || existingResume.UserId != userId)
            return NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));

        var deleted = await _resumeService.DeleteAsync(id);

        return deleted
            ? NoContent()
            : NotFound(new ErrorResponseDto("RESUME_NOT_FOUND", "Resume was not found."));
    }
}