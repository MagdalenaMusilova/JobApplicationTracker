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
            .IsRequired(false);

        modelBuilder.Entity<JobApplication>()
            .HasMany(ja => ja.StatusHistory)
            .WithOne()
            .HasForeignKey(stat => stat.JobApplicationId);
        
        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobListing)
            .WithOne()
            .HasForeignKey<JobListing>(jl => jl.JobApplicationId)
            .IsRequired(false);
        modelBuilder.Entity<JobApplication>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ja => ja.UserId);
        
        // user resume
        modelBuilder.Entity<User>()
            .HasOne(user => user.UserResume)
            .WithOne()
            .HasForeignKey<UserResume>(ur => ur.UserId)
            .IsRequired(false);

        modelBuilder.Entity<Experience>()
            .HasDiscriminator<string>("ExperienceType")
            .HasValue<WorkExperience>("WorkExperience")
            .HasValue<Education>("Education")
            .HasValue<Training>("Training")
            .HasValue<OtherExperience>("OtherExperience");

        // linking stuff in user resume to user resume
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.WorkExperiences)
            .WithOne()
            .HasForeignKey(we => we.UserResumeId)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.Education)
            .WithOne()
            .HasForeignKey(ed => ed.UserResumeId)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.Trainings)
            .WithOne()
            .HasForeignKey(tr => tr.UserResumeId)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.Skills)
            .WithOne()
            .HasForeignKey(skill => skill.UserResumeId)
            .IsRequired(false);
        
        modelBuilder.Entity<UserResume>()
            .HasMany(resume => resume.UncategorizedExperiences)
            .WithOne()
            .HasForeignKey(skill => skill.UserResumeId)
            .IsRequired(false);
        
        // skill usage must be linked to experience and skill
        modelBuilder.Entity<Experience>()
            .HasMany(xp => xp.Skills)
            .WithOne()
            .HasForeignKey(usage => usage.ExperienceId)
            .IsRequired(false);
        
        modelBuilder.Entity<ResumeSkill>()
            .HasMany(skill => skill.Usages)
            .WithOne()
            .HasForeignKey(usage => usage.SkillId)
            .IsRequired(false);
    }
}