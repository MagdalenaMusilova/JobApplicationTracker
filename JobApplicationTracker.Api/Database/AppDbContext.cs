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
    public DbSet<JAShortcut> JaShortcutView { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // job application
        modelBuilder.Entity<JAStatusEntry>()
            .HasOne(stat => stat.JAEvent)
            .WithOne(ev => ev.JAStatusEntry)
            .HasForeignKey<JAEvent>(ev => ev.JAStatusEntryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        modelBuilder.Entity<JobApplication>()
            .HasMany(ja => ja.StatusHistory)
            .WithOne()
            .HasForeignKey(stat => stat.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobListing)
            .WithOne()
            .HasForeignKey<JobListing>(jl => jl.JobApplicationId)
             .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        modelBuilder.Entity<JobApplication>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ja => ja.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // user resume
        modelBuilder.Entity<UserResume>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<UserResume>(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        modelBuilder.Entity<Experience>().UseTpcMappingStrategy();
        
        // linking stuff in user resume to user resume
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.WorkExperiences)
            .WithOne()
            .HasForeignKey(we => we.UserResumeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.Education)
            .WithOne()
            .HasForeignKey(ed => ed.UserResumeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.Trainings)
            .WithOne()
            .HasForeignKey(tr => tr.UserResumeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.Skills)
            .WithOne()
            .HasForeignKey(skill => skill.UserResumeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.UncategorizedExperiences)
            .WithOne()
            .HasForeignKey(skill => skill.UserResumeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        // skill usage must be linked to experience and skill
        modelBuilder.Entity<Experience>()
            .HasMany(xp => xp.Skills)
            .WithOne()
            .HasForeignKey(usage => usage.ExperienceId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        modelBuilder.Entity<ResumeSkill>()
            .HasMany(skill => skill.Usages)
            .WithOne()
            .HasForeignKey(usage => usage.SkillId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        
        // minimal JA view
        modelBuilder.Entity<JobApplicationMinimal>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("View_MinimalJA");
            entity.Property(jaMin => jaMin.JAId);
        });
        
        // shortcut JA view
        modelBuilder.Entity<JAShortcut>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("View_ShortcutJA");
            entity.Property(jaMin => jaMin.JAId);
        });
        
        // Configure Identity columns properly
        modelBuilder.Entity<User>()
            .Property(u => u.NormalizedUserName)
            .HasMaxLength(256);
    
        modelBuilder.Entity<User>()
            .Property(u => u.NormalizedEmail)
            .HasMaxLength(256);
    
        modelBuilder.Entity<User>()
            .Property(u => u.UserName)
            .HasMaxLength(256);
    
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(256);
    
        // Create unique indexes for these columns
        modelBuilder.Entity<User>()
            .HasIndex(u => u.NormalizedUserName)
            .IsUnique();
    
        modelBuilder.Entity<User>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

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