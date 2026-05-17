using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Moq;
using System.Text.Json;

namespace Test.Services;

public class AiResumeDataExtractorTests
{
    private readonly Mock<IAiAgentService> _mockAiAgentService;
    private readonly AiResumeDataExtractor _extractor;

    public AiResumeDataExtractorTests()
    {
        _mockAiAgentService = new Mock<IAiAgentService>();
        _extractor = new AiResumeDataExtractor(_mockAiAgentService.Object);
    }

    private static string CreateValidResumeJson()
    {
        return JsonSerializer.Serialize(new
        {
            workExperiences = new[]
            {
                new
                {
                    id = (Guid?)null,
                    startDate = "2020-01-01",
                    endDate = "2024-01-01",
                    company = "Tech Corp",
                    position = "Senior Developer",
                    jobDescription = new[] { "Developed applications", "Led team" },
                    skills = Array.Empty<object>(),
                    notes = ""
                }
            },
            education = Array.Empty<object>(),
            trainings = Array.Empty<object>(),
            skills = new[]
            {
                new
                {
                    id = (Guid?)null,
                    name = "C#",
                    aliases = new[] { "CSharp" },
                    level = "Expert",
                    weight = (string?)null,
                    notes = "",
                    skills = Array.Empty<object>()
                }
            },
            uncategorizedSkillUsages = Array.Empty<object>()
        });
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_CallsAiServiceWithPrompt()
    {
        // Arrange
        var resumeText = "John Doe - Software Engineer with 5 years experience";
        var jsonResponse = CreateValidResumeJson();

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        // Act
        await _extractor.ExtractFromPlaintextAsync(resumeText);

        // Assert
        _mockAiAgentService.Verify(
            s => s.MakeRequestAsync(It.Is<string>(prompt => prompt.Contains(resumeText))),
            Times.Once);
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_IncludesSchemaInPrompt()
    {
        // Arrange
        var resumeText = "Test resume";
        var jsonResponse = CreateValidResumeJson();

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        // Act
        await _extractor.ExtractFromPlaintextAsync(resumeText);

        // Assert
        _mockAiAgentService.Verify(
            s => s.MakeRequestAsync(It.Is<string>(prompt =>
                prompt.Contains("workExperiences") &&
                prompt.Contains("education") &&
                prompt.Contains("skills"))),
            Times.Once);
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_DeserializesJsonResponse()
    {
        // Arrange
        var resumeText = "Test resume";
        var jsonResponse = CreateValidResumeJson();

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(resumeText);

        // Assert
        result.Should().NotBeNull();
        result!.WorkExperiences.Should().HaveCount(1);
        result.Skills.Should().HaveCount(1);
        result.WorkExperiences.First().Company.Should().Be("Tech Corp");
        result.Skills.First().Name.Should().Be("C#");
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_HandlesCaseInsensitiveJson()
    {
        // Arrange
        var resumeText = "Test resume";
        var jsonResponse = @"{
            ""WORKEXPERIENCES"": [],
            ""education"": [],
            ""Trainings"": [],
            ""sKiLLs"": [],
            ""uncategorizedSkillUsages"": []
        }";

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(jsonResponse);

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(resumeText);

        // Assert
        result.Should().NotBeNull();
        result!.WorkExperiences.Should().BeEmpty();
        result.Education.Should().BeEmpty();
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_ReturnsNull_WhenJsonIsInvalid()
    {
        // Arrange
        var resumeText = "Test resume";
        var invalidJson = "This is not JSON";

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(invalidJson);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(
            async () => await _extractor.ExtractFromPlaintextAsync(resumeText));
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_ParsesComplexResume()
    {
        // Arrange
        var resumeText = "Experienced developer";
        var complexJson = JsonSerializer.Serialize(new
        {
            workExperiences = new[]
            {
                new
                {
                    id = (Guid?)null,
                    startDate = "2020-01-01",
                    endDate = (string?)null,
                    company = "Current Company",
                    position = "Lead Developer",
                    jobDescription = new[] { "Leading team", "Architecture design" },
                    skills = new[]
                    {
                        new
                        {
                            id = (Guid?)null,
                            skill = new
                            {
                                id = (Guid?)null,
                                name = "C#",
                                aliases = new[] { "CSharp" },
                                level = (string?)null,
                                weight = (string?)null,
                                notes = ""
                            },
                            description = "Used C# for backend development"
                        }
                    },
                    notes = "Current position"
                }
            },
            education = new[]
            {
                new
                {
                    id = (Guid?)null,
                    degree = "B.S. Computer Science",
                    isFinished = true,
                    school = "Tech University",
                    majors = new[] { "Computer Science" },
                    skills = Array.Empty<object>(),
                    notes = ""
                }
            },
            trainings = Array.Empty<object>(),
            skills = new[]
            {
                new
                {
                    id = (Guid?)null,
                    name = "Python",
                    aliases = new[] { "py" },
                    level = "Advanced",
                    weight = (string?)null,
                    notes = "",
                    skills = Array.Empty<object>()
                }
            },
            uncategorizedSkillUsages = Array.Empty<object>()
        });

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(complexJson);

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(resumeText);

        // Assert
        result.Should().NotBeNull();
        result!.WorkExperiences.Should().HaveCount(1);
        result.Education.Should().HaveCount(1);
        result.Skills.Should().HaveCount(1);
        
        var workExp = result.WorkExperiences.First();
        workExp.Company.Should().Be("Current Company");
        workExp.Skills.Should().HaveCount(1);
        workExp.Skills.First().Skill.Name.Should().Be("C#");

        var education = result.Education.First();
        education.Degree.Should().Be("B.S. Computer Science");
        education.IsFinished.Should().BeTrue();

        result.Skills.First().Name.Should().Be("Python");
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_PropagatesAiServiceException()
    {
        // Arrange
        var resumeText = "Test resume";

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("AI service unavailable"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await _extractor.ExtractFromPlaintextAsync(resumeText));
    }
}