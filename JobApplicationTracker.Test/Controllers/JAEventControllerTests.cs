using System.Security.Claims;
using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test.Controllers;

public class JAEventControllerTests
{
    private readonly Mock<IJobApplicationService> _mockJobApplicationService;
    private readonly Mock<IJAStatusEntryService> _mockStatusEntryService;
    private readonly Mock<IJAEventService> _mockEventService;
    private readonly JAEventController _controller;

    public JAEventControllerTests()
    {
        _mockJobApplicationService = new Mock<IJobApplicationService>();
        _mockStatusEntryService = new Mock<IJAStatusEntryService>();
        _mockEventService = new Mock<IJAEventService>();
        _controller = new JAEventController(
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