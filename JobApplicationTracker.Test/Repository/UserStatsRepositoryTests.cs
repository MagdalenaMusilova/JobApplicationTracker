using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

public class UserStatsRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CountJobApplicationsAsync_ReturnsCorrectCount()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserStatsRepository(context);
        var applications = new List<JobApplication>
        {
            new() { Company = "Company A", Position = "Developer", UserId = "user1" },
            new() { Company = "Company B", Position = "Engineer", UserId = "user2" },
            new() { Company = "Company C", Position = "Manager", UserId = "user1" }
        };
        context.JobApplications.AddRange(applications);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.CountJobApplicationsAsync();

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public async Task CountJobApplicationsAsync_ReturnsZero_WhenNoApplications()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserStatsRepository(context);

        // Act
        var result = await repository.CountJobApplicationsAsync();

        // Assert
        result.Should().Be(0);
    }
}