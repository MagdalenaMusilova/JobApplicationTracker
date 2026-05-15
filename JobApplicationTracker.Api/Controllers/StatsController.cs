using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;
    
    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }
    
    [HttpGet("usersOnlyWishlisted")]
    public async Task<IEnumerable<UserDto>> GetAllUsersWOnlyWhishlistedAsync()
    {
        var res = await _statsService.GetAllUsersWOnlyWhishlistedAsync();
        return res;
    }
    
    [HttpGet("JAWithAllStatuses")]
    public async Task<IEnumerable<JobApplicationDto>> GetAllJAWithAllStatusesAsync()
    {
        var res = await _statsService.GetAllJAWithAllStatusesAsync();
        return res;
    }
    
    [HttpGet("statusXEvent")]
    public async Task<IEnumerable<ProfileXResumeDto>> GetStatusXEventAsync()
    {
        var res = await _statsService.GetStatusXEventAsync();
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
    public async Task<IEnumerable<JobApplicationDto>> GetJANotWhishlistedAsync()
    {
        var res = await _statsService.GetJANotWhishlistedAsync();
        return res;
    }
    
    [HttpGet("JAWithMultipleStats")]
    public async Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync()
    {
        var res = await _statsService.JobApplicationsWithMoreThanOneStatusAsync();
        return res;
    }
}