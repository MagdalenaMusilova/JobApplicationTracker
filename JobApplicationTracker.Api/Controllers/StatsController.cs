using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/stats")]
[Authorize]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;
    
    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }
    
    [HttpGet("usersOnlyWishlisted")]
    public async Task<IEnumerable<UserDto>> GetAllUsersWOnlyWishlistedAsync()
    {
        var res = await _statsService.GetAllUsersWOnlyWishlistedAsync();
        return res;
    }
    
    [HttpGet("JAWithAllStatuses")]
    public async Task<IEnumerable<JobApplicationDto>> GetAllJAWithAllStatusesAsync()
    {
        var res = await _statsService.GetAllJAWithAllStatusesAsync();
        return res;
    }
    
    [HttpGet("statusXEvent")]
    public async Task<IEnumerable<UserXResumeDto>> GetUserXResumeAsync()
    {
        var res = await _statsService.GetUserXResumeAsync();
        return res;
    }
    
    [HttpGet("statusesWEventsLeftJoin")]
    public async Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsLeftJoinAsync()
    {
        var res = await _statsService.GetStatusesWEventsLeftJoinAsync();
        return res;
    }
    
    [HttpGet("statusesWEventsFullJoin")]
    public async Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsFullJoinAsync()
    {
        var res = await _statsService.GetStatusesWEventsFullJoinAsync();
        return res;
    }
    
    [HttpGet("JAWithInterviewsOrTasks")]
    public async Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsOrTasksAsync()
    {
        var res = await _statsService.GetJAWithInterviewsOrTasksAsync();
        return res;
    }
    
    [HttpGet("JAWithInterviewsAndTasks")]
    public async Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsAndTasksAsync()
    {
        var res = await _statsService.GetJAWithInterviewsAndTasksAsync();
        return res;
    }
    
    [HttpGet("JANotWhishlisted")]
    public async Task<IEnumerable<JobApplicationDto>> GetJANotWishlistedAsync()
    {
        var res = await _statsService.GetJANotWishlistedAsync();
        return res;
    }
    
    [HttpGet("JAWithMultipleStats")]
    public async Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync()
    {
        var res = await _statsService.JobApplicationsWithMoreThanOneStatusAsync();
        return res;
    }
}