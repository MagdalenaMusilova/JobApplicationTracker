using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/ai")]
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