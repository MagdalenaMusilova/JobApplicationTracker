using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

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

    private static void AssertError(object? value, string expectedCode, string expectedMessage)
    {
        var error = value.Should().BeOfType<ErrorResponseDto>().Subject;
        error.Code.Should().Be(expectedCode);
        error.Message.Should().Be(expectedMessage);
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
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");
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
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");
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
    public async Task Create_ReturnsBadRequest_WhenServiceThrowsInvalidOperationException()
    {
        // Arrange
        var userId = "user123";
        var createDto = new CreateJobApplicationDto
        {
            Company = "New Company",
            Position = "Developer"
        };

        SetupUser(userId);

        _mockJobApplicationService
            .Setup(s => s.AddAsync(userId, createDto))
            .ThrowsAsync(new InvalidOperationException("Invalid application data."));

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "APPLICATION_CREATE_FAILED", "Invalid application data.");
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
    public async Task Update_ReturnsNotFound_WhenApplicationDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var updateDto = new UpdateJobApplicationDto { Company = "New Name" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync((JobApplicationDto?)null);

        // Act
        var result = await _controller.Update(appId, updateDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");

        _mockJobApplicationService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateJobApplicationDto>()),
            Times.Never);
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
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");

        _mockJobApplicationService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateJobApplicationDto>()),
            Times.Never);
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
    public async Task Delete_ReturnsNotFound_WhenApplicationDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync((JobApplicationDto?)null);

        // Act
        var result = await _controller.Delete(appId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");

        _mockJobApplicationService.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
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
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");

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
        result.Result.Should().BeOfType<OkObjectResult>();

        _mockJobApplicationService.Verify(s => s.PushApplicationStatusAsync(
            It.Is<CreateJAStatusEntryDto>(dto =>
                dto.JobApplicationId == appId &&
                dto.StatusType == (int)JAStatusType.Rejected)), Times.Once);
    }

    [Fact]
    public async Task MarkApplicationAsRejected_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var existing = new JobApplicationDto { Id = appId, UserId = "otherUser" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);

        // Act
        var result = await _controller.MarkApplicationAsRejected(appId);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");

        _mockJobApplicationService.Verify(
            s => s.PushApplicationStatusAsync(It.IsAny<CreateJAStatusEntryDto>()),
            Times.Never);
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
        result.Result.Should().BeOfType<OkObjectResult>();
        _mockJobApplicationService.Verify(s => s.PushApplicationStatusAsync(statusEntry), Times.Once);
    }

    [Fact]
    public async Task PushApplicationStatus_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = appId,
            StatusType = (int)JAStatusType.Applied
        };
        var existing = new JobApplicationDto { Id = appId, UserId = "otherUser" };

        SetupUser(userId);
        _mockJobApplicationService.Setup(s => s.GetByIdAsync(appId)).ReturnsAsync(existing);

        // Act
        var result = await _controller.PushApplicationStatus(statusEntry);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "APPLICATION_NOT_FOUND", "Job application was not found.");

        _mockJobApplicationService.Verify(
            s => s.PushApplicationStatusAsync(It.IsAny<CreateJAStatusEntryDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateStatusEntry_UpdatesEntry_WhenUserOwnsApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var entryId = Guid.NewGuid();

        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = appId,
            StatusType = (int)JAStatusType.Applied
        };

        var existingEntry = new JAStatusEntryDto
        {
            Id = entryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = userId
        };

        var updatedEntry = new JAStatusEntryDto
        {
            Id = entryId,
            JobApplicationId = appId,
            JaStatusType = JAStatusType.Applied
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync(existingEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        _mockStatusEntryService
            .Setup(s => s.UpdateAsync(entryId, statusEntry))
            .ReturnsAsync(updatedEntry);

        // Act
        var result = await _controller.UpdateStatusEntry(entryId, statusEntry);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedEntry = okResult.Value.Should().BeOfType<JAStatusEntryDto>().Subject;
        returnedEntry.Id.Should().Be(entryId);

        _mockStatusEntryService.Verify(s => s.UpdateAsync(entryId, statusEntry), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusEntry_ReturnsNotFound_WhenEntryDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var entryId = Guid.NewGuid();

        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = Guid.NewGuid(),
            StatusType = (int)JAStatusType.Applied
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync((JAStatusEntryDto?)null);

        // Act
        var result = await _controller.UpdateStatusEntry(entryId, statusEntry);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "STATUS_ENTRY_NOT_FOUND", "Status entry was not found.");

        _mockStatusEntryService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CreateJAStatusEntryDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateStatusEntry_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var entryId = Guid.NewGuid();

        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = appId,
            StatusType = (int)JAStatusType.Applied
        };

        var existingEntry = new JAStatusEntryDto
        {
            Id = entryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = "otherUser"
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync(existingEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        // Act
        var result = await _controller.UpdateStatusEntry(entryId, statusEntry);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "STATUS_ENTRY_NOT_FOUND", "Status entry was not found.");

        _mockStatusEntryService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CreateJAStatusEntryDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateStatusEntry_ReturnsBadRequest_WhenApplicationIdChanges()
    {
        // Arrange
        var userId = "user123";
        var originalAppId = Guid.NewGuid();
        var differentAppId = Guid.NewGuid();
        var entryId = Guid.NewGuid();

        var statusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = differentAppId,
            StatusType = (int)JAStatusType.Applied
        };

        var existingEntry = new JAStatusEntryDto
        {
            Id = entryId,
            JobApplicationId = originalAppId
        };

        var application = new JobApplicationDto
        {
            Id = originalAppId,
            UserId = userId
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync(existingEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(originalAppId))
            .ReturnsAsync(application);

        // Act
        var result = await _controller.UpdateStatusEntry(entryId, statusEntry);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "APPLICATION_MISMATCH", "Status entry cannot be moved to another job application.");

        _mockStatusEntryService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CreateJAStatusEntryDto>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteJAStatusEntry_DeletesEntry_WhenUserOwnsApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var entryId = Guid.NewGuid();

        var existingEntry = new JAStatusEntryDto
        {
            Id = entryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = userId
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync(existingEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        _mockStatusEntryService
            .Setup(s => s.DeleteAsync(entryId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteJAStatusEntry(entryId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedApplication = okResult.Value.Should().BeOfType<JobApplicationDto>().Subject;
        returnedApplication.Id.Should().Be(appId);

        _mockStatusEntryService.Verify(s => s.DeleteAsync(entryId), Times.Once);
    }

    [Fact]
    public async Task DeleteJAStatusEntry_ReturnsNotFound_WhenEntryDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var entryId = Guid.NewGuid();

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync((JAStatusEntryDto?)null);

        // Act
        var result = await _controller.DeleteJAStatusEntry(entryId);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "STATUS_ENTRY_NOT_FOUND", "Status entry was not found.");

        _mockStatusEntryService.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeleteJAStatusEntry_ReturnsNotFound_WhenUserDoesNotOwnApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var entryId = Guid.NewGuid();

        var existingEntry = new JAStatusEntryDto
        {
            Id = entryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = "otherUser"
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(entryId))
            .ReturnsAsync(existingEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        // Act
        var result = await _controller.DeleteJAStatusEntry(entryId);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "STATUS_ENTRY_NOT_FOUND", "Status entry was not found.");

        _mockStatusEntryService.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task CreateEvent_CreatesEvent()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var statusEntryId = Guid.NewGuid();

        var createDto = new CreateJAEventDto
        {
            JAStatusEntryId = statusEntryId,
            EventName = "Interview",
            EventType = (int)JAEventType.Interview,
            EventDate = DateTime.UtcNow.ToString("O"),
            IsWholeDay = false
        };

        var statusEntry = new JAStatusEntryDto
        {
            Id = statusEntryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = userId
        };

        var created = new JAEventDto
        {
            Id = Guid.NewGuid(),
            JAStatusEntryId = statusEntryId,
            EventName = "Interview"
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync(statusEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        _mockEventService
            .Setup(s => s.AddAsync(createDto))
            .ReturnsAsync(created);

        // Act
        var result = await _controller.CreateEvent(createDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedEvent = okResult.Value.Should().BeOfType<JAEventDto>().Subject;
        returnedEvent.EventName.Should().Be("Interview");

        _mockEventService.Verify(s => s.AddAsync(createDto), Times.Once);
    }

    [Fact]
    public async Task CreateEvent_ReturnsNotFound_WhenStatusEntryDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var statusEntryId = Guid.NewGuid();

        var createDto = new CreateJAEventDto
        {
            JAStatusEntryId = statusEntryId,
            EventName = "Interview",
            EventType = (int)JAEventType.Interview,
            EventDate = DateTime.UtcNow.ToString("O"),
            IsWholeDay = false
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync((JAStatusEntryDto?)null);

        // Act
        var result = await _controller.CreateEvent(createDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "STATUS_ENTRY_NOT_FOUND", "Status entry was not found.");

        _mockEventService.Verify(s => s.AddAsync(It.IsAny<CreateJAEventDto>()), Times.Never);
    }

    [Fact]
    public async Task CreateEvent_ReturnsNotFound_WhenUserDoesNotOwnStatusEntryApplication()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var statusEntryId = Guid.NewGuid();

        var createDto = new CreateJAEventDto
        {
            JAStatusEntryId = statusEntryId,
            EventName = "Interview",
            EventType = (int)JAEventType.Interview,
            EventDate = DateTime.UtcNow.ToString("O"),
            IsWholeDay = false
        };

        var statusEntry = new JAStatusEntryDto
        {
            Id = statusEntryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = "otherUser"
        };

        SetupUser(userId);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync(statusEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        // Act
        var result = await _controller.CreateEvent(createDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "STATUS_ENTRY_NOT_FOUND", "Status entry was not found.");

        _mockEventService.Verify(s => s.AddAsync(It.IsAny<CreateJAEventDto>()), Times.Never);
    }

    [Fact]
    public async Task UpdateEvent_UpdatesEvent_WhenUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var statusEntryId = Guid.NewGuid();

        var updateDto = new UpdateJAEventDto { EventName = "Updated Interview" };

        var existingEvent = new JAEventDto
        {
            Id = eventId,
            JAStatusEntryId = statusEntryId,
            EventName = "Interview"
        };

        var statusEntry = new JAStatusEntryDto
        {
            Id = statusEntryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = userId
        };

        var updated = new JAEventDto
        {
            Id = eventId,
            JAStatusEntryId = statusEntryId,
            EventName = "Updated Interview"
        };

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync(statusEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        _mockEventService
            .Setup(s => s.UpdateAsync(eventId, updateDto))
            .ReturnsAsync(updated);

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
        var userId = "user123";
        var eventId = Guid.NewGuid();
        var updateDto = new UpdateJAEventDto { EventName = "Updated" };

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetByIdAsync(eventId))
            .ReturnsAsync((JAEventDto?)null);

        // Act
        var result = await _controller.UpdateEvent(eventId, updateDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "EVENT_NOT_FOUND", "Event was not found.");

        _mockEventService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateJAEventDto>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateEvent_ReturnsNotFound_WhenUserDoesNotOwnEvent()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var statusEntryId = Guid.NewGuid();
        var updateDto = new UpdateJAEventDto { EventName = "Updated Interview" };

        var existingEvent = new JAEventDto
        {
            Id = eventId,
            JAStatusEntryId = statusEntryId,
            EventName = "Interview"
        };

        var statusEntry = new JAStatusEntryDto
        {
            Id = statusEntryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = "otherUser"
        };

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync(statusEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        // Act
        var result = await _controller.UpdateEvent(eventId, updateDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "EVENT_NOT_FOUND", "Event was not found.");

        _mockEventService.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateJAEventDto>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteEvent_DeletesEvent_WhenUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var statusEntryId = Guid.NewGuid();

        var existingEvent = new JAEventDto
        {
            Id = eventId,
            JAStatusEntryId = statusEntryId,
            EventName = "Interview"
        };

        var statusEntry = new JAStatusEntryDto
        {
            Id = statusEntryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = userId
        };

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync(statusEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        _mockEventService
            .Setup(s => s.DeleteBulkAsync(It.Is<IEnumerable<Guid>>(ids => ids.Contains(eventId))))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mockEventService.Verify(
            s => s.DeleteBulkAsync(It.Is<IEnumerable<Guid>>(ids => ids.Contains(eventId))),
            Times.Once);
    }

    [Fact]
    public async Task DeleteEvent_ReturnsNotFound_WhenEventDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var eventId = Guid.NewGuid();

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetByIdAsync(eventId))
            .ReturnsAsync((JAEventDto?)null);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "EVENT_NOT_FOUND", "Event was not found.");

        _mockEventService.Verify(
            s => s.DeleteBulkAsync(It.IsAny<IEnumerable<Guid>>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteEvent_ReturnsNotFound_WhenUserDoesNotOwnEvent()
    {
        // Arrange
        var userId = "user123";
        var appId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var statusEntryId = Guid.NewGuid();

        var existingEvent = new JAEventDto
        {
            Id = eventId,
            JAStatusEntryId = statusEntryId,
            EventName = "Interview"
        };

        var statusEntry = new JAStatusEntryDto
        {
            Id = statusEntryId,
            JobApplicationId = appId
        };

        var application = new JobApplicationDto
        {
            Id = appId,
            UserId = "otherUser"
        };

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetByIdAsync(eventId))
            .ReturnsAsync(existingEvent);

        _mockStatusEntryService
            .Setup(s => s.GetByIdAsync(statusEntryId))
            .ReturnsAsync(statusEntry);

        _mockJobApplicationService
            .Setup(s => s.GetByIdAsync(appId))
            .ReturnsAsync(application);

        // Act
        var result = await _controller.DeleteEvent(eventId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "EVENT_NOT_FOUND", "Event was not found.");

        _mockEventService.Verify(
            s => s.DeleteBulkAsync(It.IsAny<IEnumerable<Guid>>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAllUserEvents_ReturnsEvents_WhenRequestedUserIsCurrentUser()
    {
        // Arrange
        var userId = "user123";
        var events = new List<JAEventDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                JAStatusEntryId = Guid.NewGuid(),
                EventName = "Interview"
            }
        };

        SetupUser(userId);

        _mockEventService
            .Setup(s => s.GetAllByUserId(userId))
            .ReturnsAsync(events);

        // Act
        var result = await _controller.GetAllUserEvents(userId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedEvents = okResult.Value.Should().BeAssignableTo<IEnumerable<JAEventDto>>().Subject;
        returnedEvents.Should().HaveCount(1);

        _mockEventService.Verify(s => s.GetAllByUserId(userId), Times.Once);
    }

    [Fact]
    public async Task GetAllUserEvents_ReturnsNotFound_WhenRequestedUserIsNotCurrentUser()
    {
        // Arrange
        SetupUser("user123");

        // Act
        var result = await _controller.GetAllUserEvents("otherUser");

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "EVENTS_NOT_FOUND", "Events were not found.");

        _mockEventService.Verify(s => s.GetAllByUserId(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetAllUserEvents_ReturnsBadRequest_WhenUserIdIsEmpty()
    {
        // Arrange
        SetupUser("user123");

        // Act
        var result = await _controller.GetAllUserEvents("");

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "USER_ID_REQUIRED", "User ID is required.");

        _mockEventService.Verify(s => s.GetAllByUserId(It.IsAny<string>()), Times.Never);
    }
}