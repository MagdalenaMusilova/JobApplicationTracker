using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

public class JAStatusEntryRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsStatusEntry_WhenExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);
        var statusId = Guid.NewGuid();
        var statusEntry = new JAStatusEntry
        {
            Id = statusId,
            JobApplicationId = Guid.NewGuid(),
            JaStatusType = JAStatusType.Applied,
            Note = "Test note"
        };
        context.JAStatusEntries.Add(statusEntry);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(statusId);

        // Assert
        result.Should().NotBeNull();
        result!.JaStatusType.Should().Be(JAStatusType.Applied);
        result.Note.Should().Be("Test note");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByJobApplicationIdsAsync_ReturnsMatchingEntries()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);
        var jaId1 = Guid.NewGuid();
        var jaId2 = Guid.NewGuid();
        var jaId3 = Guid.NewGuid();
        var statusEntries = new List<JAStatusEntry>
        {
            new() { Id = Guid.NewGuid(), JobApplicationId = jaId1, JaStatusType = JAStatusType.Applied },
            new() { Id = Guid.NewGuid(), JobApplicationId = jaId1, JaStatusType = JAStatusType.Interview },
            new() { Id = Guid.NewGuid(), JobApplicationId = jaId2, JaStatusType = JAStatusType.Applied },
            new() { Id = Guid.NewGuid(), JobApplicationId = jaId3, JaStatusType = JAStatusType.Rejected }
        };
        context.JAStatusEntries.AddRange(statusEntries);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByJobApplicationIdsAsync(new[] { jaId1, jaId2 });

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(e => e.JobApplicationId == jaId1);
        result.Should().Contain(e => e.JobApplicationId == jaId2);
        result.Should().NotContain(e => e.JobApplicationId == jaId3);
    }

    [Fact]
    public async Task AddAsync_AddsStatusEntrySuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);
        var statusEntry = new JAStatusEntry
        {
            JobApplicationId = Guid.NewGuid(),
            JaStatusType = JAStatusType.Interview,
            Note = "First round interview"
        };

        // Act
        var result = await repository.AddAsync(statusEntry);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().NotBeEmpty();
        var savedEntry = await context.JAStatusEntries.FindAsync(result.Id);
        savedEntry.Should().NotBeNull();
        savedEntry!.JaStatusType.Should().Be(JAStatusType.Interview);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesStatusEntrySuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);
        var statusEntry = new JAStatusEntry
        {
            JobApplicationId = Guid.NewGuid(),
            JaStatusType = JAStatusType.Applied,
            Note = "Initial note"
        };
        context.JAStatusEntries.Add(statusEntry);
        await context.SaveChangesAsync();

        var updatedEntry = new JAStatusEntry
        {
            Id = statusEntry.Id,
            JaStatusType = JAStatusType.Interview,
            Note = "Updated note"
        };

        // Act
        var result = await repository.UpdateAsync(statusEntry.Id, updatedEntry);

        // Assert
        result.Should().NotBeNull();
        result!.JaStatusType.Should().Be(JAStatusType.Interview);
        result.Note.Should().Be("Updated note");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);
        var updatedEntry = new JAStatusEntry
        {
            Id = Guid.NewGuid(),
            JaStatusType = JAStatusType.Interview
        };

        // Act
        var result = await repository.UpdateAsync(Guid.NewGuid(), updatedEntry);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteBulkAsync_DeletesMultipleEntries()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new JAStatusEntryRepository(context);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        var statusEntries = new List<JAStatusEntry>
        {
            new() { Id = id1, JobApplicationId = Guid.NewGuid(), JaStatusType = JAStatusType.Applied },
            new() { Id = id2, JobApplicationId = Guid.NewGuid(), JaStatusType = JAStatusType.Interview },
            new() { Id = id3, JobApplicationId = Guid.NewGuid(), JaStatusType = JAStatusType.Rejected }
        };
        context.JAStatusEntries.AddRange(statusEntries);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteBulkAsync(new[] { id1, id2 });

        // Assert
        result.Should().BeTrue();
        var remainingEntries = await context.JAStatusEntries.ToListAsync();
        remainingEntries.Should().HaveCount(1);
        remainingEntries[0].Id.Should().Be(id3);
    }
}