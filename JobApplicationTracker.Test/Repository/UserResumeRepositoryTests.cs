using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models.UserProfile;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Test.Repository;

public class UserResumeRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private IMapper CreateMockMapper()
    {
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<List<WorkExperience>>(It.IsAny<List<WorkExperience>>()))
            .Returns<List<WorkExperience>>(src => src);
        mockMapper.Setup(m => m.Map<List<Education>>(It.IsAny<List<Education>>()))
            .Returns<List<Education>>(src => src);
        mockMapper.Setup(m => m.Map<List<Training>>(It.IsAny<List<Training>>()))
            .Returns<List<Training>>(src => src);
        mockMapper.Setup(m => m.Map<List<ResumeSkill>>(It.IsAny<List<ResumeSkill>>()))
            .Returns<List<ResumeSkill>>(src => src);
        return mockMapper.Object;
    }

    [Fact]
    public async Task CreateAsync_CreatesResumeSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);
        var resume = new UserResume
        {
            UserId = "user1",
            AboutMe = "Test about me"
        };

        // Act
        var result = await repository.CreateAsync(resume);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        var savedResume = await context.ResumeEntries.FindAsync(result.Id);
        savedResume.Should().NotBeNull();
        savedResume!.AboutMe.Should().Be("Test about me");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsResume_WhenExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);
        var resumeId = Guid.NewGuid();
        var resume = new UserResume
        {
            Id = resumeId,
            UserId = "user1",
            AboutMe = "Test resume"
        };
        context.ResumeEntries.Add(resume);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(resumeId);

        // Assert
        result.Should().NotBeNull();
        result!.AboutMe.Should().Be("Test resume");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsResume_WhenExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);
        var resume = new UserResume
        {
            UserId = "user1",
            AboutMe = "User's resume"
        };
        context.ResumeEntries.Add(resume);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByUserAsync("user1");

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be("user1");
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);

        // Act
        var result = await repository.GetByUserAsync("nonexistent");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesResumeSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);
        var resume = new UserResume
        {
            UserId = "user1",
            AboutMe = "Original about me",
            WorkExperiences = new List<WorkExperience>(),
            Education = new List<Education>(),
            Trainings = new List<Training>(),
            Skills = new List<ResumeSkill>(),
            UncategorizedExperiences = new List<OtherExperience>()
        };
        context.ResumeEntries.Add(resume);
        await context.SaveChangesAsync();

        resume.AboutMe = "Updated about me";

        // Act
        var result = await repository.UpdateAsync(resume);

        // Assert
        result.Should().NotBeNull();
        result!.AboutMe.Should().Be("Updated about me");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);
        var resume = new UserResume
        {
            Id = Guid.NewGuid(),
            UserId = "user1"
        };

        // Act
        var result = await repository.UpdateAsync(resume);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DeletesResumeSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var mapper = CreateMockMapper();
        var repository = new UserResumeRepository(context, mapper);
        var resume = new UserResume
        {
            UserId = "user1",
            AboutMe = "Test resume"
        };
        context.ResumeEntries.Add(resume);
        await context.SaveChangesAsync();
        var resumeId = resume.Id;

        // Act
        var result = await repository.DeleteAsync(resumeId);

        // Assert
        result.Should().BeTrue();
        var deletedResume = await context.ResumeEntries.FindAsync(resumeId);
        deletedResume.Should().BeNull();
    }
}