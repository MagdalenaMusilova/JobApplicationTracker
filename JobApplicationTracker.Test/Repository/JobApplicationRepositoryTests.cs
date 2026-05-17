using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

public class JobApplicationRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllJobApplications()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var applications = new List<JobApplication>
        {
            new() { Company = "Company A", Position = "Developer", UserId = "user1" },
            new() { Company = "Company B", Position = "Engineer", UserId = "user2" }
        };
        context.JobApplications.AddRange(applications);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllNotFinishedAsync_ReturnsOnlyUnfinishedApplications()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var finishedApp = new JobApplication
        {
            Company = "Company A",
            Position = "Developer",
            UserId = "user1",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Rejected, OrderIndex = 1 }
            }
        };
        var unfinishedApp = new JobApplication
        {
            Company = "Company B",
            Position = "Engineer",
            UserId = "user2",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Applied, OrderIndex = 1 }
            }
        };
        context.JobApplications.AddRange(finishedApp, unfinishedApp);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllNotFinishedAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Company.Should().Be("Company B");
    }

    [Fact]
    public async Task GetAllByUserAsync_ReturnsUserApplicationsWithStatusHistory()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var userId = "user1";
        var applications = new List<JobApplication>
        {
            new() { Company = "Company A", Position = "Developer", UserId = userId, StatusHistory = new List<JAStatusEntry>() },
            new() { Company = "Company B", Position = "Engineer", UserId = userId, StatusHistory = new List<JAStatusEntry>() },
            new() { Company = "Company C", Position = "Manager", UserId = "user2", StatusHistory = new List<JAStatusEntry>() }
        };
        context.JobApplications.AddRange(applications);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllByUserAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(ja => ja.UserId == userId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsApplication_WhenExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var appId = Guid.NewGuid();
        var application = new JobApplication
        {
            Id = appId,
            Company = "Test Company",
            Position = "Test Position",
            UserId = "user1",
            StatusHistory = new List<JAStatusEntry>
            {
                new() { JaStatusType = JAStatusType.Applied }
            }
        };
        context.JobApplications.Add(application);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(appId);

        // Assert
        result.Should().NotBeNull();
        result!.Company.Should().Be("Test Company");
        result.StatusHistory.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_AddsApplicationSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var application = new JobApplication
        {
            Company = "New Company",
            Position = "New Position",
            UserId = "user1",
            Note = "Test note"
        };

        // Act
        var result = await repository.AddAsync(application);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        var savedApp = await context.JobApplications.FindAsync(result.Id);
        savedApp.Should().NotBeNull();
        savedApp!.Company.Should().Be("New Company");
    }

    [Fact]
    public async Task UpdateAsync_UpdatesApplicationSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var application = new JobApplication
        {
            Company = "Original Company",
            Position = "Original Position",
            UserId = "user1",
            StatusHistory = new List<JAStatusEntry>()
        };
        context.JobApplications.Add(application);
        await context.SaveChangesAsync();

        application.Company = "Updated Company";
        application.Position = "Updated Position";
        application.Note = "Updated note";

        // Act
        var result = await repository.UpdateAsync(application);

        // Assert
        result.Should().NotBeNull();
        result!.Company.Should().Be("Updated Company");
        result.Position.Should().Be("Updated Position");
        result.Note.Should().Be("Updated note");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var application = new JobApplication
        {
            Id = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            UserId = "user1"
        };

        // Act
        var result = await repository.UpdateAsync(application);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DeletesApplicationSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);
        var application = new JobApplication
        {
            Company = "Test Company",
            Position = "Test Position",
            UserId = "user1"
        };
        context.JobApplications.Add(application);
        await context.SaveChangesAsync();
        var appId = application.Id;

        // Act
        var result = await repository.DeleteAsync(appId);

        // Assert
        result.Should().BeTrue();
        var deletedApp = await context.JobApplications.FindAsync(appId);
        deletedApp.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JobApplicationRepository(context);

        // Act
        var result = await repository.DeleteAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }
}