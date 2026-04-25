using JobApplicationTracker.Models;
using JobApplicationTracker.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Database;

public class UserResumeDbContext: DbContext
{
    public DbSet<UserResume> ResumeEntries { get; set; }

    public UserResumeDbContext(DbContextOptions<UserResumeDbContext> options) : base(options)
    {
    }
}