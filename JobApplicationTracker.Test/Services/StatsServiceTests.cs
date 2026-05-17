using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class StatsServiceTests
{
    private readonly Mock<IStatsRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly StatsService _service;

    public StatsServiceTests()
    {
        _mockRepository = new Mock<IStatsRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new StatsService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllUsersWOnlyWhishlistedAsync_ReturnsMappedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = "user1", UserName = "alice" }
        };
        var userDtos = new List<UserDto>
        {
            new() { Id = "user1", UserName = "alice" }
        };

        _mockRepository.Setup(r => r.GetAllUsersWOnlyWhishlistedAsync()).ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
            .Returns((User u) => new UserDto { Id = u.Id, UserName = u.UserName });

        // Act
        var result = await _service.GetAllUsersWOnlyWhishlistedAsync();

        // Assert
        result.Should().HaveCount(1);
        _mockRepository.Verify(r => r.GetAllUsersWOnlyWhishlistedAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllJAWithAllStatusesAsync_ReturnsMappedApplications()
    {
        // Arrange
        var applications = new List<JobApplication>
        {
            new() { Id = Guid.NewGuid(), Company = "Company A" }
        };

        _mockRepository.Setup(r => r.GetAllJAWithAllStatusesAsync()).ReturnsAsync(applications);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(It.IsAny<JobApplication>()))
            .Returns((JobApplication ja) => new JobApplicationDto { Id = ja.Id });

        // Act
        var result = await _service.GetAllJAWithAllStatusesAsync();

        // Assert
        result.Should().HaveCount(1);
        _mockRepository.Verify(r => r.GetAllJAWithAllStatusesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetStatusXEventAsync_ReturnsProfileXResumeData()
    {
        // Arrange
        var data = new List<ProfileXResumeDto>
        {
            new() { Username = "alice", AboutMe = "Resume 1" }
        };

        _mockRepository.Setup(r => r.GetStatusXEventAsync()).ReturnsAsync(data);

        // Act
        var result = await _service.GetStatusXEventAsync();

        // Assert
        result.Should().HaveCount(1);
        _mockRepository.Verify(r => r.GetStatusXEventAsync(), Times.Once);
    }

    [Fact]
    public async Task GetJANotWhishlistedAsync_ReturnsMappedApplications()
    {
        // Arrange
        var applications = new List<JobApplication>
        {
            new() { Id = Guid.NewGuid(), Company = "Company A" }
        };

        _mockRepository.Setup(r => r.GetJANotWhishlistedAsync()).ReturnsAsync(applications);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(It.IsAny<JobApplication>()))
            .Returns((JobApplication ja) => new JobApplicationDto { Id = ja.Id });

        // Act
        var result = await _service.GetJANotWhishlistedAsync();

        // Assert
        result.Should().HaveCount(1);
        _mockRepository.Verify(r => r.GetJANotWhishlistedAsync(), Times.Once);
    }

    [Fact]
    public async Task JobApplicationsWithMoreThanOneStatusAsync_ReturnsApplicationCounts()
    {
        // Arrange
        var counts = new List<ApplicationCountDto>
        {
            new() { ApplicationId = Guid.NewGuid(), Count = 3 }
        };

        _mockRepository.Setup(r => r.JobApplicationsWithMoreThanOneStatusAsync()).ReturnsAsync(counts);

        // Act
        var result = await _service.JobApplicationsWithMoreThanOneStatusAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Count.Should().Be(3);
        _mockRepository.Verify(r => r.JobApplicationsWithMoreThanOneStatusAsync(), Times.Once);
    }
}