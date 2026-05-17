using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Models.UserProfile;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

public class StatsRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllUsersWOnlyWhishlistedAsync_ReturnsOnlyUsersWithOnlyWishlisted()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new StatsRepository(context);
        
        var user1 = new User { Id = "user1", UserName = "user1", Email = "user1@test.com" };
        var user2 = new User { Id = "user2", UserName = "user2", Email = "user2@test.com" };
        context.Users.AddRange(user1, user2);

        var app1 = new JobApplication
        {
            UserId = "user1",
            Company = "Company A",
            Position = "Developer",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Whishlist, OrderIndex = 1 }
            }
        };
        var app2 = new JobApplication
        {
            UserId = "user2",
            Company = "Company B",
            Position = "Engineer",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Whishlist, OrderIndex = 1 },
                new() { JaStatusType = JAStatusType.Applied, OrderIndex = 2 }
            }
        };
        context.JobApplications.AddRange(app1, app2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllUsersWOnlyWhishlistedAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Id.Should().Be("user1");
    }

    [Fact]
    public async Task GetJANotWhishlistedAsync_ReturnsNonWishlistedApplications()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new StatsRepository(context);

        var wishlisted = new JobApplication
        {
            Company = "Company A",
            Position = "Developer",
            UserId = "user1",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Whishlist, OrderIndex = 1 }
            }
        };
        var applied = new JobApplication
        {
            Company = "Company B",
            Position = "Engineer",
            UserId = "user2",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Applied, OrderIndex = 1 }
            }
        };
        context.JobApplications.AddRange(wishlisted, applied);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetJANotWhishlistedAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Company.Should().Be("Company B");
    }

    [Fact]
    public async Task JobApplicationsWithMoreThanOneStatusAsync_ReturnsCorrectCounts()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new StatsRepository(context);

        var app1Id = Guid.NewGuid();
        var app2Id = Guid.NewGuid();

        var statusEntries = new List<JAStatusEntry>
        {
            new() { JobApplicationId = app1Id, JaStatusType = JAStatusType.Applied },
            new() { JobApplicationId = app1Id, JaStatusType = JAStatusType.Interview },
            new() { JobApplicationId = app1Id, JaStatusType = JAStatusType.Offer },
            new() { JobApplicationId = app2Id, JaStatusType = JAStatusType.Applied }
        };
        context.JAStatusEntries.AddRange(statusEntries);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.JobApplicationsWithMoreThanOneStatusAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().ApplicationId.Should().Be(app1Id);
        result.First().Count.Should().Be(3);
    }

    [Fact]
    public async Task GetStatusXEventAsync_ReturnsCrossProduct()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new StatsRepository(context);

        context.Users.AddRange(
            new User { Id = "user1", UserName = "alice", Email = "alice@test.com" },
            new User { Id = "user2", UserName = "bob", Email = "bob@test.com" }
        );
        context.ResumeEntries.AddRange(
            new UserResume { UserId = "user1", AboutMe = "Resume 1" },
            new UserResume { UserId = "user2", AboutMe = "Resume 2" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetStatusXEventAsync();

        // Assert
        result.Should().HaveCount(4); // 2 users * 2 resumes
    }
}