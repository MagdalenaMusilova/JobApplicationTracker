using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/resume")]
public class ResumeController : ControllerBase
{
    private readonly IResumeService _resumeService;
    
    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
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

        var result = await _resumeService.ExtractFromPdf(file);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public Task<ActionResult<UserResumeDto>> GetById(int id)
    {
        //todo
        throw new NotImplementedException();
    }

    [HttpGet("user={userId:int}")]
    public Task<ActionResult<UserResumeDto>> GetByUserId(int userId)
    {
        //todo
        throw new NotImplementedException();
    }
}