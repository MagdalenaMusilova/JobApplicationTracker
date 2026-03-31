using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Database;

public class JobApplicationDbContext : DbContext
{
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JAStatusEntry> JAStatusEntries { get; set; }

    public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>()
            .HasMany(j => j.StatusHistory)
            .WithOne()
            .HasForeignKey(s => s.JobApplicationId);

        base.OnModelCreating(modelBuilder);
    }
}