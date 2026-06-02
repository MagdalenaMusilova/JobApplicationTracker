using JobApplicationTracker.Models;
using JobApplicationTracker.Models.UserProfile;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserResume> ResumeEntries { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JAStatusEntry> JAStatusEntries { get; set; }
    public DbSet<JAEvent> JAEventEntries { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<JobApplicationMinimal> JaMinimalView { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // job application

        // ... existing code ...

        modelBuilder.Entity<User>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .HasOne(refreshToken => refreshToken.User)
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(refreshToken => refreshToken.Token)
            .IsUnique();
    }
}