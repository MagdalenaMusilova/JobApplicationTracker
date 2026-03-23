using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Database;

public class JobApplicationDbContext : DbContext
{
    public DbSet<JobApplication> JobApplications { get; set; }

    public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : base(options)
    {
    }
}