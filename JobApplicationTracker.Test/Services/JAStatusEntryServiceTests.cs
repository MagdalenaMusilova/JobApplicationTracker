using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class JAStatusEntryServiceTests
{
    private readonly Mock<IJAStatusEntryRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly JAStatusEntryService _service;

    public JAStatusEntryServiceTests()
    {
        _mockRepository = new Mock<IJAStatusEntryRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new JAStatusEntryService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsStatusEntry_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entry = new JAStatusEntry { Id = id, JaStatusType = JAStatusType.Applied };
        var entryDto = new JAStatusEntryDto { Id = id, JaStatusType = JAStatusType.Applied };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entry);
        _mockMapper.Setup(m => m.Map<JAStatusEntryDto>(entry)).Returns(entryDto);

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
        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((JAStatusEntry?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByJobApplicationIdsAsync_ReturnsMatchingEntries()
    {
        // Arrange
        var jaId1 = Guid.NewGuid();
        var jaId2 = Guid.NewGuid();
        var entries = new List<JAStatusEntry>
        {
            new() { Id = Guid.NewGuid(), JobApplicationId = jaId1, JaStatusType = JAStatusType.Applied },
            new() { Id = Guid.NewGuid(), JobApplicationId = jaId2, JaStatusType = JAStatusType.Interview }
        };
        var entryDtos = entries.Select(e => new JAStatusEntryDto { Id = e.Id }).ToList();

        _mockRepository.Setup(r => r.GetByJobApplicationIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(entries);
        _mockMapper.Setup(m => m.Map<JAStatusEntryDto>(It.IsAny<JAStatusEntry>()))
            .Returns((JAStatusEntry e) => new JAStatusEntryDto { Id = e.Id });

        // Act
        var result = await _service.GetByJobApplicationIdsAsync(new[] { jaId1, jaId2 });

        // Assert
        result.Should().HaveCount(2);
        _mockRepository.Verify(r => r.GetByJobApplicationIdsAsync(It.IsAny<IEnumerable<Guid>>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_CreatesStatusEntrySuccessfully()
    {
        // Arrange
        var jobAppDto = new JobApplicationDto
        {
            Id = Guid.NewGuid(),
            StatusHistory = new List<JAStatusEntryDto>()
        };
        var createDto = new CreateJAStatusEntryDto
        {
            JobApplicationId = jobAppDto.Id,
            StatusType = (int)JAStatusType.Applied,
            Note = "Test note"
        };
        var createdEntry = new JAStatusEntry
        {
            Id = Guid.NewGuid(),
            JobApplicationId = jobAppDto.Id,
            JaStatusType = JAStatusType.Applied,
            Note = "Test note"
        };
        var createdDto = new JAStatusEntryDto { Id = createdEntry.Id };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<JAStatusEntry>())).ReturnsAsync(createdEntry);
        _mockMapper.Setup(m => m.Map<JAStatusEntryDto>(createdEntry)).Returns(createdDto);

        // Act
        var result = await _service.AddAsync(jobAppDto, createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(createdEntry.Id);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<JAStatusEntry>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesSuccessfully_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new JAStatusEntry { Id = id, JaStatusType = JAStatusType.Applied };
        var updateDto = new CreateJAStatusEntryDto
        {
            StatusType = (int)JAStatusType.Interview,
            Note = "Updated note"
        };
        var updated = new JAStatusEntry { Id = id, JaStatusType = JAStatusType.Interview };
        var updatedDto = new JAStatusEntryDto { Id = id };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _mockRepository.Setup(r => r.UpdateAsync(id, It.IsAny<JAStatusEntry>())).ReturnsAsync(updated);
        _mockMapper.Setup(m => m.Map<JAStatusEntryDto>(updated)).Returns(updatedDto);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().NotBeNull();
        _mockRepository.Verify(r => r.UpdateAsync(id, It.IsAny<JAStatusEntry>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview };

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((JAStatusEntry?)null);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<JAStatusEntry>()), Times.Never);
    }

    [Fact]
    public async Task DeleteBulkAsync_DeletesMultipleEntries()
    {
        // Arrange
        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
        _mockRepository.Setup(r => r.DeleteBulkAsync(ids)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteBulkAsync(ids);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteBulkAsync(ids), Times.Once);
    }
}