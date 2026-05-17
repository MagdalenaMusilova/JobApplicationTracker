using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Controllers;

public class StatsControllerTests
{
    private readonly Mock<IStatsService> _mockStatsService;
    private readonly StatsController _controller;

    public StatsControllerTests()
    {
        _mockStatsService = new Mock<IStatsService>();
        _controller = new StatsController(_mockStatsService.Object);
    }

    [Fact]
    public async Task GetAllUsersWOnlyWhishlistedAsync_ReturnsUsers()
    {
        // Arrange
        var users = new List<UserDto>
        {
            new() { Id = "user1", UserName = "alice" },
            new() { Id = "user2", UserName = "bob" }
        };
        _mockStatsService.Setup(s => s.GetAllUsersWOnlyWhishlistedAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllUsersWOnlyWhishlistedAsync();

        // Assert
        result.Should().HaveCount(2);
        _mockStatsService.Verify(s => s.GetAllUsersWOnlyWhishlistedAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllJAWithAllStatusesAsync_ReturnsApplications()
    {
        // Arrange
        var applications = new List<JobApplicationDto>
        {
            new() { Id = Guid.NewGuid(), Company = "Company A" }
        };
        _mockStatsService.Setup(s => s.GetAllJAWithAllStatusesAsync()).ReturnsAsync(applications);

        // Act
        var result = await _controller.GetAllJAWithAllStatusesAsync();

        // Assert
        result.Should().HaveCount(1);
        _mockStatsService.Verify(s => s.GetAllJAWithAllStatusesAsync(), Times.Once);
    }

    [Fact]
    public async Task JobApplicationsWithMoreThanOneStatusAsync_ReturnsCounts()
    {
        // Arrange
        var counts = new List<ApplicationCountDto>
        {
            new() { ApplicationId = Guid.NewGuid(), Count = 3 }
        };
        _mockStatsService.Setup(s => s.JobApplicationsWithMoreThanOneStatusAsync()).ReturnsAsync(counts);

        // Act
        var result = await _controller.JobApplicationsWithMoreThanOneStatusAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Count.Should().Be(3);
        _mockStatsService.Verify(s => s.JobApplicationsWithMoreThanOneStatusAsync(), Times.Once);
    }
}