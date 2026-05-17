using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class JobApplicationServiceTests
{
    private readonly Mock<IJobApplicationRepository> _mockRepository;
    private readonly Mock<IJAStatusEntryService> _mockStatusEntryService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly JobApplicationService _service;

    public JobApplicationServiceTests()
    {
        _mockRepository = new Mock<IJobApplicationRepository>();
        _mockStatusEntryService = new Mock<IJAStatusEntryService>();
        _mockMapper = new Mock<IMapper>();
        _service = new JobApplicationService(
            _mockRepository.Object,
            _mockStatusEntryService.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllApplications()
    {
        // Arrange
        var applications = new List<JobApplication>
        {
            new() { Id = Guid.NewGuid(), Company = "Company A" },
            new() { Id = Guid.NewGuid(), Company = "Company B" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(applications);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(It.IsAny<JobApplication>()))
            .Returns((JobApplication ja) => new JobApplicationDto { Id = ja.Id, Company = ja.Company });

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllByUserAsync_ReturnsUserApplications()
    {
        // Arrange
        var userId = "user1";
        var applications = new List<JobApplication>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Company = "Company A" }
        };

        _mockRepository.Setup(r => r.GetAllByUserAsync(userId)).ReturnsAsync(applications);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(It.IsAny<JobApplication>()))
            .Returns((JobApplication ja) => new JobApplicationDto { Id = ja.Id });

        // Act
        var result = await _service.GetAllByUserAsync(userId);

        // Assert
        result.Should().HaveCount(1);
        _mockRepository.Verify(r => r.GetAllByUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsApplication_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var application = new JobApplication { Id = id, Company = "Test Company" };
        var applicationDto = new JobApplicationDto { Id = id, Company = "Test Company" };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(application);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(application)).Returns(applicationDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((JobApplication?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task AddAsync_CreatesApplicationWithInitialStatus()
    {
        // Arrange
        var userId = "user1";
        var createDto = new CreateJobApplicationDto
        {
            Company = "New Company",
            Position = "Developer",
            InitialStatus = new CreateJAStatusEntryDto
            {
                StatusType = (int)JAStatusType.Wishlist
            }
        };
        var created = new JobApplication
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Company = "New Company",
            StatusHistory = new List<JAStatusEntry>()
        };
        var createdDto = new JobApplicationDto { Id = created.Id, StatusHistory = new List<JAStatusEntryDto>() };
        var withStatus = new JobApplication { Id = created.Id, StatusHistory = new List<JAStatusEntry> { new() } };
        var finalDto = new JobApplicationDto { Id = created.Id };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<JobApplication>())).ReturnsAsync(created);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(created)).Returns(createdDto);
        _mockStatusEntryService.Setup(s => s.AddAsync(It.IsAny<JobApplicationDto>(), It.IsAny<CreateJAStatusEntryDto>()))
            .ReturnsAsync(new JAStatusEntryDto());
        _mockRepository.Setup(r => r.GetByIdAsync(created.Id)).ReturnsAsync(withStatus);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(withStatus)).Returns(finalDto);

        // Act
        var result = await _service.AddAsync(userId, createDto);

        // Assert
        result.Should().NotBeNull();
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<JobApplication>()), Times.Once);
        _mockStatusEntryService.Verify(s => s.AddAsync(It.IsAny<JobApplicationDto>(), It.IsAny<CreateJAStatusEntryDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesSuccessfully_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new JobApplication
        {
            Id = id,
            UserId = "user1",
            Company = "Original",
            Position = "Developer",
            StatusHistory = new List<JAStatusEntry>()
        };
        var updateDto = new UpdateJobApplicationDto { Company = "Updated" };
        var updated = new JobApplication { Id = id, Company = "Updated" };
        var updatedDto = new JobApplicationDto { Id = id };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<JobApplication>())).ReturnsAsync(updated);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(updated)).Returns(updatedDto);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().NotBeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateJobApplicationDto { Company = "Updated" };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((JobApplication?)null);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<JobApplication>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task PushApplicationStatusAsync_AddsNewStatus()
    {
        // Arrange
        var appId = Guid.NewGuid();
        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = appId,
            StatusType = (int)JAStatusType.Applied
        };
        var application = new JobApplication { Id = appId, StatusHistory = new List<JAStatusEntry>() };
        var applicationDto = new JobApplicationDto { Id = appId, StatusHistory = new List<JAStatusEntryDto>() };
        var updatedApp = new JobApplication { Id = appId, StatusHistory = new List<JAStatusEntry> { new() } };
        var updatedDto = new JobApplicationDto { Id = appId };

        _mockRepository.SetupSequence(r => r.GetByIdAsync(appId))
            .ReturnsAsync(application)
            .ReturnsAsync(updatedApp);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(application)).Returns(applicationDto);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(updatedApp)).Returns(updatedDto);
        _mockStatusEntryService.Setup(s => s.AddAsync(applicationDto, statusEntry))
            .ReturnsAsync(new JAStatusEntryDto());

        // Act
        var result = await _service.PushApplicationStatusAsync(statusEntry);

        // Assert
        result.Should().NotBeNull();
        _mockStatusEntryService.Verify(s => s.AddAsync(It.IsAny<JobApplicationDto>(), statusEntry), Times.Once);
    }

    [Fact]
    public async Task DeleteJAStatusEntryAsync_ThrowsException_WhenApplicationNotFound()
    {
        // Arrange
        var entryId = Guid.NewGuid();
        var statusEntry = new JAStatusEntryDto { Id = entryId, JobApplicationId = Guid.NewGuid() };

        _mockStatusEntryService.Setup(s => s.GetByIdAsync(entryId)).ReturnsAsync(statusEntry);
        _mockRepository.Setup(r => r.GetByIdAsync(statusEntry.JobApplicationId))
            .ReturnsAsync((JobApplication?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteJAStatusEntryAsync(entryId));
    }

    [Fact]
    public async Task DeleteJAStatusEntryAsync_ThrowsException_WhenLastStatus()
    {
        // Arrange
        var entryId = Guid.NewGuid();
        var appId = Guid.NewGuid();
        var statusEntry = new JAStatusEntryDto { Id = entryId, JobApplicationId = appId };
        var application = new JobApplication
        {
            Id = appId,
            StatusHistory = new List<JAStatusEntry> { new() { Id = entryId } }
        };
        var applicationDto = new JobApplicationDto
        {
            Id = appId,
            StatusHistory = new List<JAStatusEntryDto> { statusEntry }
        };

        _mockStatusEntryService.Setup(s => s.GetByIdAsync(entryId)).ReturnsAsync(statusEntry);
        _mockRepository.Setup(r => r.GetByIdAsync(appId)).ReturnsAsync(application);
        _mockMapper.Setup(m => m.Map<JobApplicationDto>(application)).Returns(applicationDto);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteJAStatusEntryAsync(entryId));
    }
}