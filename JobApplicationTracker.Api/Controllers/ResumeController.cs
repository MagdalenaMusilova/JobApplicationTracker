using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/resume")]
[Authorize]
public class ResumeController : ControllerBase
{
    private readonly IResumeService _resumeService;
    
    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserResumeDto>> Create([FromBody] UserResumeDto resume)
    {
        var created = await _resumeService.CreateAsync(resume);
        return Ok(created);
    }
    
    [HttpPost("extract")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ExtractFromPdf([FromForm] PdfUploadRequestDto request)
    {
        var file = request.File;
        
        if (file == null || file.Length == 0)
            return BadRequest("Please upload a PDF file.");

        if (Path.GetExtension(file.FileName).ToLowerInvariant() != ".pdf")
            return BadRequest("Only PDF files are allowed.");

        var result = await _resumeService.ExtractFromPdfAsync(file);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResumeDto>> GetById(Guid id)
    {
        var resume = await _resumeService.GetByIdAsync(id);
        if (resume is null) return NotFound();
        return Ok(resume);   
    }

    [HttpGet("user={userId:guid}")]
    public async Task<ActionResult<UserResumeDto>> GetByUserId(Guid userId)
    {
        var resume = await _resumeService.GetByUserAsync(userId);
        if (resume is null) return NotFound();
        return Ok(resume);   
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResumeDto>> Update(Guid id, [FromBody] UserResumeDto resume)
    {
        var updated = await _resumeService.UpdateAsync(id, resume);
        return updated is null ? NotFound() : Ok(updated);  
    }
    
    [HttpPut("{id:guid}/merge")]
    public async Task<ActionResult<UserResumeDto>> Merge(Guid id, [FromForm] PdfUploadRequestDto request)
    {
        var extracted = await _resumeService.ExtractFromPdfAsync(request.File);
        var merged = await _resumeService.MergeAsync(id, extracted);
        return merged is null ? NotFound() : Ok(merged);  
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _resumeService.DeleteAsync(id);
        return NoContent();
    }
}