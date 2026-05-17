using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using JobApplicationTracker.Models;

namespace Test.Controllers;

public class JobApplicationControllerTests
{
    private readonly Mock<IJobApplicationService> _mockJobApplicationService;
    private readonly Mock<IJAStatusEntryService> _mockStatusEntryService;
    private readonly Mock<IJAEventService> _mockEventService;
    private readonly JobApplicationController _controller;

    public JobApplicationControllerTests()
    {
        _mockJobApplicationService = new Mock<IJobApplicationService>();
        _mockStatusEntryService = new Mock<IJAStatusEntryService>();
        _mockEventService = new Mock<IJAEventService>();
        _controller = new JobApplicationController(
            _mockJobApplicationService.Object,
            _mockStatusEntryService.Object,
            _mockEventService.Object);
    }

    private void SetupUser(string userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Fact]
    public async Task GetAllByUserAsync_ReturnsUserApplications()
    {
        // Arrange
        var userId = "user123";
        var applications = new List<JobApplicationDto>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Company = "Company A" }
        };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetAllByUserAsync(userId)).ReturnsAsync(applications);

        // Act
        var result = await _controller.GetAllByUserAsync();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedApps = okResult.Value.Should().BeAssignableTo<IEnumerable<JobApplicationDto>>().Subject;
        returnedApps.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetById_ReturnsApplication_WhenUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var application = new JobApplicationDto { Id = appId, UserId = userId, Company = "Test Co" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(application);

        // Act
        var result = await _controller.GetById(appId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedApp = okResult.Value.Should().BeOfType<JobApplicationDto>().Subject;
        returnedApp.Id.Should().Be(appId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenApplicationDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync((JobApplicationDto?)null);

        // Act
        var result = await _controller.GetById(appId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var application = new JobApplicationDto { Id = appId, UserId = "otherUser", Company = "Test Co" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(application);

        // Act
        var result = await _controller.GetById(appId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_CreatesApplication()
    {
        // Arrange
        var userId = "user123";
        var createDto = new CreateJobApplicationDto
        {
            Company = "New Company",
            Position = "Developer",
            InitialStatus = new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Wishlist }
        };
        var created = new JobApplicationDto { Id = Guid.NewGuid(), UserId = userId, Company = "New Company" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.AddAsync(userId, createDto)).ReturnsAsync(created);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedApp = okResult.Value.Should().BeOfType<JobApplicationDto>().Subject;
        returnedApp.Company.Should().Be("New Company");
    }

    [Fact]
    public async Task Update_UpdatesApplication_WhenUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var existing = new JobApplicationDto { Id = appId, UserId = userId, Company = "Old Name" };
        var updateDto = new UpdateJobApplicationDto { Company = "New Name" };
        var updated = new JobApplicationDto { Id = appId, UserId = userId, Company = "New Name" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);
        _mockJobApplicationService.Setup(s => s.UpdateAsync(appId, updateDto)).ReturnsAsync(updated);

        // Act
        var result = await _controller.Update(appId, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedApp = okResult.Value.Should().BeOfType<JobApplicationDto>().Subject;
        returnedApp.Company.Should().Be("New Name");
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var existing = new JobApplicationDto { Id = appId, UserId = "otherUser" };
        var updateDto = new UpdateJobApplicationDto { Company = "New Name" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);

        // Act
        var result = await _controller.Update(appId, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _mockJobApplicationService.Verify(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateJobApplicationDto>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DeletesApplication_WhenUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var existing = new JobApplicationDto { Id = appId, UserId = userId };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);
        _mockJobApplicationService.Setup(s => s.DeleteAsync(appId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(appId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockJobApplicationService.Verify(s => s.DeleteAsync(appId), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var existing = new JobApplicationDto { Id = appId, UserId = "otherUser" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);

        // Act
        var result = await _controller.Delete(appId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _mockJobApplicationService.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task MarkApplicationAsRejected_UpdatesStatus()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var existing = new JobApplicationDto { Id = appId, UserId = userId };
        var updated = new JobApplicationDto { Id = appId, UserId = userId };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);
        _mockJobApplicationService.Setup(s => s.PushApplicationStatusAsync(It.IsAny<CreateJAStatusEntryDto>()))
            .ReturnsAsync(updated);

        // Act
        var result = await _controller.MarkApplicationAsRejected(appId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        _mockJobApplicationService.Verify(s => s.PushApplicationStatusAsync(
            It.Is<CreateJAStatusEntryDto>(dto => dto.StatusType == (int)JAStatusType.Rejected)), Times.Once);
    }

    [Fact]
    public async Task PushApplicationStatus_AddsStatus_WhenUserOwnsApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = appId,
            StatusType = (int)JAStatusType.Applied
        };
        var existing = new JobApplicationDto { Id = appId, UserId = userId };
        var updated = new JobApplicationDto { Id = appId, UserId = userId };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);
        _mockJobApplicationService.Setup(s => s.PushApplicationStatusAsync(statusEntry)).ReturnsAsync(updated);

        // Act
        var result = await _controller.PushApplicationStatus(statusEntry);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        _mockJobApplicationService.Verify(s => s.PushApplicationStatusAsync(statusEntry), Times.Once);
    }

    [Fact]
    public async Task DeleteJAStatusEntry_ReturnsConflict_WhenInvalidOperation()
    {
        // Arrange
        var entryId = Guid.NewGuid();
        _mockJobApplicationService.Setup(s => s.DeleteJAStatusEntryAsync(entryId))
            .ThrowsAsync(new InvalidOperationException("Cannot delete last status"));

        // Act
        var result = await _controller.DeleteJAStatusEntry(entryId);

        // Assert
        var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflictResult.Value.Should().Be("Cannot delete last status");
    }

    [Fact]
    public async Task CreateEvent_CreatesEvent()
    {
        // Arrange
        var createDto = new CreateJAEventDto
        {
            JAStatusEntryId = Guid.NewGuid(),
            EventName = "Interview",
            EventType = (int)JAEventType.Interview,
            EventDate = DateTime.UtcNow.ToString("O"),
            IsWholeDay = false
        };
        var created = new JAEventDto { Id = Guid.NewGuid(), EventName = "Interview" };

        _mockEventService.Setup(s => s.AddAsync(createDto)).ReturnsAsync(created);

        // Act
        var result = await _controller.CreateEvent(createDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedEvent = okResult.Value.Should().BeOfType<JAEventDto>().Subject;
        returnedEvent.EventName.Should().Be("Interview");
    }

    [Fact]
    public async Task UpdateEvent_UpdatesEvent_WhenExists()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var updateDto = new UpdateJAEventDto { EventName = "Updated Interview" };
        var updated = new JAEventDto { Id = eventId, EventName = "Updated Interview" };

        _mockEventService.Setup(s => s.UpdateAsync(eventId, updateDto)).ReturnsAsync(updated);

        // Act
        var result = await _controller.UpdateEvent(eventId, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedEvent = okResult.Value.Should().BeOfType<JAEventDto>().Subject;
        returnedEvent.EventName.Should().Be("Updated Interview");
    }

    [Fact]
    public async Task UpdateEvent_ReturnsNotFound_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var updateDto = new UpdateJAEventDto { EventName = "Updated" };

        _mockEventService.Setup(s => s.UpdateAsync(eventId, updateDto)).ReturnsAsync((JAEventDto?)null);

        // Act
        var result = await _controller.UpdateEvent(eventId, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteEvent_DeletesEvent_WhenExists()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var existing = new JAEventDto() { Id = eventId };

        _mockEventService.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync(existing);
        _mockEventService.Setup(s => s.DeleteBulkAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockEventService.Verify(s => s.DeleteBulkAsync(It.Is<IEnumerable<Guid>>(ids => ids.Contains(eventId))), Times.Once);
    }

    [Fact]
    public async Task DeleteEvent_ReturnsNotFound_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        _mockEventService.Setup(s => s.GetByIdAsync(eventId)).ReturnsAsync((JAEventDto?)null);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _mockEventService.Verify(s => s.DeleteBulkAsync(It.IsAny<IEnumerable<Guid>>()), Times.Never);
    }
}