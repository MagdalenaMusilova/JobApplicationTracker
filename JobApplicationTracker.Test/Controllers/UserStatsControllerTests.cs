using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Controllers;

public class UserStatsControllerTests
{
    private readonly Mock<IUserStatsService> _mockUserStatsService;
    private readonly UserStatsController _controller;

    public UserStatsControllerTests()
    {
        _mockUserStatsService = new Mock<IUserStatsService>();
        _controller = new UserStatsController(_mockUserStatsService.Object);
    }

    [Fact]
    public async Task CountJobApplicationsAsync_ReturnsCount()
    {
        // Arrange
        _mockUserStatsService.Setup(s => s.CountJobApplicationsAsync()).ReturnsAsync(42);

        // Act
        var result = await _controller.CountJobApplicationsAsync();

        // Assert
        result.Should().Be(42);
        _mockUserStatsService.Verify(s => s.CountJobApplicationsAsync(), Times.Once);
    }
}