using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/userStats")]
[Authorize]
public class UserStatsController : ControllerBase
{
    private readonly IUserStatsService _userStatsService;
    
    public UserStatsController(IUserStatsService userStatsService)
    {
        _userStatsService = userStatsService;
    }
    
    [HttpGet("countJA")]
    public async Task<int> CountJobApplicationsAsync()
    {
        return await _userStatsService.CountJobApplicationsAsync();
    }
}