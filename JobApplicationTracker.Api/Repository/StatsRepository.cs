using JobApplicationTracker.Database;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class StatsRepository : IStatsRepository
{
    private readonly AppDbContext _context;

    public StatsRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersWOnlyWishlistedAsync()
    {
        var res = await _context.Users
            .Where(u =>
                _context.JobApplications
                    .Where(ja => ja.UserId == u.Id)
                    .All(ja => ja.StatusHistory.Count == 1 &&
                                 ja.StatusHistory.First().JaStatusType == JAStatusType.Wishlist)
            )
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JobApplication>> GetAllJAWithAllStatusesAsync()
    {
        var res = await _context.JobApplications
            .Where(ja =>
                Enum.GetValues<JAStatusType>()
                    .All(statType => ja.StatusHistory.Any(stat => stat.JaStatusType == statType))
            )
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<UserXResumeDto>> GetUserXResumeAsync()
    {
        var res = await _context.Users
            .SelectMany(u => _context.ResumeEntries,
                (u, resume) => new UserXResumeDto
                {
                    Username = u.UserName,
                    AboutMe = resume.AboutMe
                })
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JAStatusEntry>> GetStatusesWEventsLeftJoinAsync()
    {
        var res = await _context.JAStatusEntries
            .FromSqlRaw(@"
                SELECT s.*, e.*
                FROM  JAStatusEntries s
                LEFT OUTER JOIN JAEventEntries e
                    ON e.JAStatusEntryId = s.Id
            ").ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JAStatusEntry>> GetStatusesWEventsFullJoinAsync()
    {
        var res = await _context.JAStatusEntries
            .FromSqlRaw(@"
                SELECT s.*, e.*
                FROM  JAStatusEntries s
                FULL OUTER JOIN JAEventEntries e
                    ON e.JAStatusEntryId = s.Id
            ").ToListAsync();
        return res;
        
    }

    public async Task<IEnumerable<JobApplication>> GetJAWithInterviewsOrTasksAsync()
    {
        var wInterview = _context.JobApplications
            .Where(ja => ja.StatusHistory
                .Any(stat => stat.JAEvent.EventType == JAEventType.Interview));
        var wTask = _context.JobApplications
            .Where(ja => ja.StatusHistory
                .Any(stat => stat.JAEvent.EventType == JAEventType.Task));
        var res = await wInterview.Union(wTask).ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JobApplication>> GetJAWithInterviewsAndTasksAsync()
    {
        var wInterview = _context.JobApplications
            .Where(ja => ja.StatusHistory
                .Any(stat => stat.JAEvent.EventType == JAEventType.Interview));
        var wTask = _context.JobApplications
            .Where(ja => ja.StatusHistory
                .Any(stat => stat.JAEvent.EventType == JAEventType.Task));
        var res = await wInterview.Intersect(wTask).ToListAsync();
        return res;
        
    }

    public async Task<IEnumerable<JobApplication>> GetJANotWishlistedAsync()
    {
        var wishlisted = _context.JobApplications
            .Where(ja => 
                ja.StatusHistory
                    .OrderByDescending(stat => stat.OrderIndex)
                    .FirstOrDefault().JaStatusType == JAStatusType.Wishlist
                );
        var res = await _context.JobApplications.Except(wishlisted).ToListAsync();
        return res;
    }

    public async Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync()
    {
        var res = await _context.JAStatusEntries
            .GroupBy(s => s.JobApplicationId)
            .Where(g => g.Count() > 1)
            .Select(g => new ApplicationCountDto()
            {
                ApplicationId = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
        return res;
    }
}