using FluentAssertions;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class UserStatsServiceTests
{
    private readonly Mock<IUserStatsRepository> _mockRepository;
    private readonly UserStatsService _service;

    public UserStatsServiceTests()
    {
        _mockRepository = new Mock<IUserStatsRepository>();
        _service = new UserStatsService(_mockRepository.Object);
    }

    [Fact]
    public async Task CountJobApplicationsAsync_ReturnsCorrectCount()
    {
        // Arrange
        _mockRepository.Setup(r => r.CountJobApplicationsAsync()).ReturnsAsync(42);

        // Act
        var result = await _service.CountJobApplicationsAsync();

        // Assert
        result.Should().Be(42);
        _mockRepository.Verify(r => r.CountJobApplicationsAsync(), Times.Once);
    }

    [Fact]
    public async Task CountJobApplicationsAsync_ReturnsZero_WhenNoApplications()
    {
        // Arrange
        _mockRepository.Setup(r => r.CountJobApplicationsAsync()).ReturnsAsync(0);

        // Act
        var result = await _service.CountJobApplicationsAsync();

        // Assert
        result.Should().Be(0);
        _mockRepository.Verify(r => r.CountJobApplicationsAsync(), Times.Once);
    }
}