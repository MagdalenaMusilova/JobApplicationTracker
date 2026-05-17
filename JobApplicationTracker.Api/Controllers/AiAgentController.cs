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
        var response = await _aiAgentService.MakeRequestAsync(prompt);
        
        return Ok(response);
    }
}