using JobApplicationTracker.Database;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class UserStatsRepository : IUserStatsRepository
{
    private readonly AppDbContext _context;

    public UserStatsRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public Task<int> CountJobApplicationsAsync()
    {
        var res = _context.JobApplications.CountAsync();
        return res;
    }
}