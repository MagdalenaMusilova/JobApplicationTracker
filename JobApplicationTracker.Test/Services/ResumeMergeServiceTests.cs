using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class ResumeMergeServiceTests
{
    private readonly Mock<IAiAgentService> _mockAiAgentService;
    private readonly ResumeMergeService _service;

    public ResumeMergeServiceTests()
    {
        _mockAiAgentService = new Mock<IAiAgentService>();
        _service = new ResumeMergeService(_mockAiAgentService.Object);
    }

    [Fact]
    public async Task MergeAsync_PreservesExistingIdAndUserId()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var existingUserId = "id";
        
        var existing = new UserResumeDto { Id = existingId, UserId = existingUserId };
        var incoming = new UserResumeDto();

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        result.Id.Should().Be(existingId);
        result.UserId.Should().Be(existingUserId);
    }

    [Fact]
    public async Task MergeAsync_AddsNewWorkExperience()
    {
        // Arrange
        var existing = new UserResumeDto
        {
            WorkExperiences = new List<WorkExperienceDto>
            {
                new() { Company = "Company A", StartDate = new DateOnly(2020, 1, 1) }
            }
        };
        
        var incoming = new UserResumeDto
        {
            WorkExperiences = new List<WorkExperienceDto>
            {
                new() { Company = "Company B", StartDate = new DateOnly(2022, 1, 1) }
            }
        };

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        result.WorkExperiences.Should().HaveCount(2);
        result.WorkExperiences.Should().Contain(we => we.Company == "Company A");
        result.WorkExperiences.Should().Contain(we => we.Company == "Company B");
    }

    [Fact]
    public async Task MergeAsync_MergesSkillsCaseInsensitively()
    {
        // Arrange
        var existing = new UserResumeDto
        {
            Skills = new List<JobSkillDto>
            {
                new() { Name = "C#", Aliases = new[] { "CSharp" } }
            }
        };
        
        var incoming = new UserResumeDto
        {
            Skills = new List<JobSkillDto>
            {
                new() { Name = "c#", Aliases = new[] { "DotNet" } }
            }
        };

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        result.Skills.Should().HaveCount(1);
        var skill = result.Skills.First();
        skill.Aliases.Should().Contain("CSharp");
        skill.Aliases.Should().Contain("DotNet");
    }

    [Fact]
    public async Task MergeAsync_DoesNotDuplicateSkillUsages()
    {
        // Arrange
        var skillUsage = new SkillUsageDto
        {
            Skill = new JobSkillDto { Name = "C#" },
            Description = "Used for backend development"
        };

        var existing = new UserResumeDto
        {
            UncategorizedSkillUsages = new List<SkillUsageDto> { skillUsage }
        };
        
        var incoming = new UserResumeDto
        {
            UncategorizedSkillUsages = new List<SkillUsageDto> { skillUsage }
        };

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        result.UncategorizedSkillUsages.Should().HaveCount(1);
    }

    [Fact]
    public async Task MergeAsync_AddsNewEducation()
    {
        // Arrange
        var existing = new UserResumeDto
        {
            Education = new List<EducationDto>
            {
                new() { School = "University A", Degree = "BS" }
            }
        };
        
        var incoming = new UserResumeDto
        {
            Education = new List<EducationDto>
            {
                new() { School = "University B", Degree = "MS" }
            }
        };

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        result.Education.Should().HaveCount(2);
    }

    [Fact]
    public async Task MergeAsync_AddsNewTrainings()
    {
        // Arrange
        var existing = new UserResumeDto
        {
            Trainings = new List<TrainingDto>
            {
                new() { Name = "Training A", StartDate = new DateOnly(2020, 1, 1) }
            }
        };
        
        var incoming = new UserResumeDto
        {
            Trainings = new List<TrainingDto>
            {
                new() { Name = "Training B", StartDate = new DateOnly(2021, 1, 1) }
            }
        };

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        result.Trainings.Should().HaveCount(2);
    }

    [Fact]
    public async Task MergeAsync_UsesAiToMergeNotes_WhenDifferent()
    {
        // Arrange
        var existing = new UserResumeDto
        {
            WorkExperiences = new List<WorkExperienceDto>
            {
                new()
                {
                    Company = "Tech Corp",
                    StartDate = new DateOnly(2020, 1, 1),
                    Notes = "Led development team"
                }
            }
        };
        
        var incoming = new UserResumeDto
        {
            WorkExperiences = new List<WorkExperienceDto>
            {
                new()
                {
                    Company = "Tech Corp",
                    StartDate = new DateOnly(2020, 1, 1),
                    Notes = "Managed cloud infrastructure"
                }
            }
        };

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync("Led development team and managed cloud infrastructure");

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        var workExp = result.WorkExperiences.First();
        workExp.Notes.Should().Be("Led development team and managed cloud infrastructure");
        _mockAiAgentService.Verify(s => s.MakeRequestAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task MergeAsync_SkipsAiCall_WhenNotesAreSubstring()
    {
        // Arrange
        var existing = new UserResumeDto
        {
            WorkExperiences = new List<WorkExperienceDto>
            {
                new()
                {
                    Company = "Tech Corp",
                    StartDate = new DateOnly(2020, 1, 1),
                    Notes = "Led development team and managed infrastructure"
                }
            }
        };
        
        var incoming = new UserResumeDto
        {
            WorkExperiences = new List<WorkExperienceDto>
            {
                new()
                {
                    Company = "Tech Corp",
                    StartDate = new DateOnly(2020, 1, 1),
                    Notes = "Led development team"
                }
            }
        };

        // Act
        var result = await _service.MergeAsync(existing, incoming);

        // Assert
        _mockAiAgentService.Verify(s => s.MakeRequestAsync(It.IsAny<string>()), Times.Never);
    }
}