using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IJAEventService _jaEventService;

    public DashboardController(
        IJobApplicationService jobApplicationService,
        IJAEventService jaEventService)
    {
        _jobApplicationService = jobApplicationService;
        _jaEventService = jaEventService;
    }

    private bool TryGetUserId(out string userId)
    {
        userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        return !string.IsNullOrWhiteSpace(userId);
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        var applications = await _jobApplicationService.GetAllByUserAsync(userId);

        var totalApplications = applications.Count();
        var activeApplications = applications.Count(a =>
            a.StatusHistory.Last().JaStatusType < JAStatusType.Accepted);

        var now = DateTime.UtcNow;
        var weekAgo = now.AddDays(-7);
        var monthAgo = now.AddMonths(-1);

        var applicationsThisWeek = applications.Count(a => a.StatusHistory.Last().CreatedAt >= weekAgo);
        var applicationsThisMonth = applications.Count(a => a.StatusHistory.Last().CreatedAt >= monthAgo);

        var applicationsByStatus = applications
            .GroupBy(a => a.StatusHistory.Last().JaStatusType)
            .ToDictionary(g => (int)g.Key, g => g.Count());

        var stats = new DashboardStatsDto
        {
            TotalApplications = totalApplications,
            ActiveApplications = activeApplications,
            InterviewsScheduled = applicationsByStatus.GetValueOrDefault((int)JAStatusType.Interview, 0),
            OffersReceived = applicationsByStatus.GetValueOrDefault((int)JAStatusType.Offer, 0),
            ApplicationsByStatus = applicationsByStatus,
            ApplicationsThisWeek = applicationsThisWeek,
            ApplicationsThisMonth = applicationsThisMonth
        };

        return Ok(stats);
    }

    [HttpGet("recent")]
    public async Task<ActionResult<List<JobApplicationMinimalDto>>> GetRecent()
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        var applications = await _jobApplicationService.GetAllByUserAsync(userId);

        var recent = applications
            .OrderByDescending(a => a.StatusHistory.Last().CreatedAt)
            .Take(5)
            .Select(a => new JobApplicationMinimalDto
            {
                JAId = a.Id,
                Company = a.Company,
                Position = a.Position,
                JAStatus = a.StatusHistory.Last().JaStatusType,
                EventType = null,
                EventDate = null,
                IsWholeDay = null,
                UpdatedAt = a.StatusHistory.Last().CreatedAt
            })
            .ToList();

        return Ok(recent);
    }
}
