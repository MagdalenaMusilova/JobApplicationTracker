using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

public class JAEventRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEvent_WhenExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);
        var eventId = Guid.NewGuid();
        var jaEvent = new JAEvent
        {
            Id = eventId,
            EventName = "Technical Interview",
            EventType = JAEventType.Interview,
            EventDate = DateTime.UtcNow.AddDays(3),
            IsWholeDay = false
        };
        context.JAEventEntries.Add(jaEvent);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(eventId);

        // Assert
        result.Should().NotBeNull();
        result!.EventName.Should().Be("Technical Interview");
        result.EventType.Should().Be(JAEventType.Interview);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByStatusIdsAsync_ReturnsMatchingEvents()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var events = new List<JAEvent>
        {
            new() { Id = id1, EventName = "Event 1", EventType = JAEventType.Interview },
            new() { Id = id2, EventName = "Event 2", EventType = JAEventType.Task },
            new() { Id = Guid.NewGuid(), EventName = "Event 3", EventType = JAEventType.Interview }
        };
        context.JAEventEntries.AddRange(events);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByStatusIdsAsync(new[] { id1, id2 });

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(e => e.EventName == "Event 1");
        result.Should().Contain(e => e.EventName == "Event 2");
    }

    [Fact]
    public async Task AddAsync_AddsEventSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);
        var jaEvent = new JAEvent
        {
            EventName = "Coding Challenge",
            EventType = JAEventType.Task,
            EventDate = DateTime.UtcNow.AddDays(1),
            IsWholeDay = true,
            Note = "Complete within 48 hours"
        };

        // Act
        var result = await repository.AddAsync(jaEvent);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        var savedEvent = await context.JAEventEntries.FindAsync(result.Id);
        savedEvent.Should().NotBeNull();
        savedEvent!.EventName.Should().Be("Coding Challenge");
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEventSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);
        var jaEvent = new JAEvent
        {
            EventName = "Initial Name",
            EventType = JAEventType.Interview,
            EventDate = DateTime.UtcNow,
            IsWholeDay = false
        };
        context.JAEventEntries.Add(jaEvent);
        await context.SaveChangesAsync();

        jaEvent.EventName = "Updated Name";
        jaEvent.EventType = JAEventType.Task;
        jaEvent.Note = "Updated note";

        // Act
        var result = await repository.UpdateAsync(jaEvent);

        // Assert
        result.Should().NotBeNull();
        result!.EventName.Should().Be("Updated Name");
        result.EventType.Should().Be(JAEventType.Task);
        result.Note.Should().Be("Updated note");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenEventNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);
        var jaEvent = new JAEvent
        {
            Id = Guid.NewGuid(),
            EventName = "Non-existent Event"
        };

        // Act
        var result = await repository.UpdateAsync(jaEvent);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteBulkAsync_DeletesMultipleEvents()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAEventRepository(context);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        var events = new List<JAEvent>
        {
            new() { Id = id1, EventName = "Event 1" },
            new() { Id = id2, EventName = "Event 2" },
            new() { Id = id3, EventName = "Event 3" }
        };
        context.JAEventEntries.AddRange(events);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteBulkAsync(new[] { id1, id2 });

        // Assert
        result.Should().BeTrue();
        var remainingEvents = await context.JAEventEntries.ToListAsync();
        remainingEvents.Should().HaveCount(1);
        remainingEvents[0].Id.Should().Be(id3);
    }
}