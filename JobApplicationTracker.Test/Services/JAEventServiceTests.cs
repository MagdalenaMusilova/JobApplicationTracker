using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class JAEventServiceTests
{
    private readonly Mock<IJAEventRepository> _mockEventRepository;
    private readonly Mock<IJobApplicationService> _mockJobApplicationService;
    private readonly Mock<IJAStatusEntryService> _mockStatusEntryService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly JAEventService _service;

    public JAEventServiceTests()
    {
        _mockEventRepository = new Mock<IJAEventRepository>();
        _mockJobApplicationService = new Mock<IJobApplicationService>();
        _mockStatusEntryService = new Mock<IJAStatusEntryService>();
        _mockMapper = new Mock<IMapper>();
        _service = new JAEventService(
            _mockEventRepository.Object,
            _mockJobApplicationService.Object,
            _mockStatusEntryService.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEvent_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var jaEvent = new JAEvent { Id = id, EventName = "Interview", EventType = JAEventType.Interview };
        var eventDto = new JAEventDto { Id = id, EventName = "Interview" };

        _mockEventRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(jaEvent);
        _mockMapper.Setup(m => m.Map<JAEventDto>(jaEvent)).Returns(eventDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        _mockEventRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByStatusIdsAsync_ReturnsMatchingEvents()
    {
        // Arrange
        var statusIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        var events = new List<JAEvent>
        {
            new() { Id = Guid.NewGuid(), EventName = "Event 1" },
            new() { Id = Guid.NewGuid(), EventName = "Event 2" }
        };

        _mockEventRepository.Setup(r => r.GetByStatusIdsAsync(statusIds)).ReturnsAsync(events);
        _mockMapper.Setup(m => m.Map<JAEventDto>(It.IsAny<JAEvent>()))
            .Returns((JAEvent e) => new JAEventDto { Id = e.Id, EventName = e.EventName });

        // Act
        var result = await _service.GetByStatusIdsAsync(statusIds);

        // Assert
        result.Should().HaveCount(2);
        _mockEventRepository.Verify(r => r.GetByStatusIdsAsync(statusIds), Times.Once);
    }

    [Fact]
    public async Task AddAsync_CreatesEventSuccessfully()
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
        var createdEvent = new JAEvent
        {
            Id = Guid.NewGuid(),
            EventName = "Interview",
            EventType = JAEventType.Interview
        };
        var eventDto = new JAEventDto { Id = createdEvent.Id };

        _mockEventRepository.Setup(r => r.AddAsync(It.IsAny<JAEvent>())).ReturnsAsync(createdEvent);
        _mockMapper.Setup(m => m.Map<JAEventDto>(createdEvent)).Returns(eventDto);

        // Act
        var result = await _service.AddAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEvent.Id);
        _mockEventRepository.Verify(r => r.AddAsync(It.IsAny<JAEvent>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesSuccessfully_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new JAEvent
        {
            Id = id,
            JAStatusEntryId = Guid.NewGuid(),
            EventName = "Original",
            EventType = JAEventType.Interview,
            EventDate = DateTime.UtcNow,
            IsWholeDay = false
        };
        var updateDto = new UpdateJAEventDto { EventName = "Updated" };
        var updated = new JAEvent { Id = id, EventName = "Updated" };
        var eventDto = new JAEventDto { Id = id };

        _mockEventRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _mockEventRepository.Setup(r => r.UpdateAsync(It.IsAny<JAEvent>())).ReturnsAsync(updated);
        _mockMapper.Setup(m => m.Map<JAEventDto>(updated)).Returns(eventDto);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().NotBeNull();
        _mockEventRepository.Verify(r => r.UpdateAsync(It.IsAny<JAEvent>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateJAEventDto { EventName = "Updated" };

        _mockEventRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((JAEvent?)null);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().BeNull();
        _mockEventRepository.Verify(r => r.UpdateAsync(It.IsAny<JAEvent>()), Times.Never);
    }

    [Fact]
    public async Task DeleteBulkAsync_DeletesMultipleEvents()
    {
        // Arrange
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        _mockEventRepository.Setup(r => r.DeleteBulkAsync(ids)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteBulkAsync(ids);

        // Assert
        result.Should().BeTrue();
        _mockEventRepository.Verify(r => r.DeleteBulkAsync(ids), Times.Once);
    }
}