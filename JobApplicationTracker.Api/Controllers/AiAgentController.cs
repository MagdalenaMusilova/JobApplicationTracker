using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AiAgentController : ControllerBase
{
    private readonly IAiAgentService _aiAgentService;

    public AiAgentController(IAiAgentService aiAgentService)
    {
        _aiAgentService = aiAgentService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> MakeRequest([FromBody] string prompt)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        if (string.IsNullOrWhiteSpace(prompt))
        {
            return BadRequest(new ErrorResponseDto("PROMPT_REQUIRED", "Prompt is required."));
        }

        try
        {
            var response = await _aiAgentService.MakeRequestAsync(prompt);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponseDto("AI_REQUEST_FAILED", ex.Message));
        }
    }
}