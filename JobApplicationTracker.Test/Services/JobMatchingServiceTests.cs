using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class JobMatchingServiceTests
{
    private readonly Mock<IAiAgentService> _mockAiAgentService;
    private readonly JobMatchingService _service;

    public JobMatchingServiceTests()
    {
        _mockAiAgentService = new Mock<IAiAgentService>();
        _service = new JobMatchingService(_mockAiAgentService.Object);
    }

    [Fact]
    public async Task EvaluateMatch_CallsAiServiceWithPrompt()
    {
        // Arrange
        var resume = new UserResumeDto
        {
            Skills = new List<JobSkillDto> { new() { Name = "C#" } }
        };
        var jobListing = "Senior C# Developer needed";
        var expectedResponse = "{\"overall_match\":{\"level\":4}}";

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.EvaluateMatch(resume, jobListing);

        // Assert
        result.Should().Be(expectedResponse);
        _mockAiAgentService.Verify(
            s => s.MakeRequestAsync(It.Is<string>(p =>
                p.Contains(jobListing) &&
                p.Contains("overall_match") &&
                p.Contains("section_scores"))),
            Times.Once);
    }

    [Fact]
    public async Task EvaluateMatch_IncludesResumeInPrompt()
    {
        // Arrange
        var resume = new UserResumeDto
        {
            Skills = new List<JobSkillDto>
            {
                new() { Name = "C#" },
                new() { Name = "Python" }
            }
        };
        var jobListing = "Test job";

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync("{}");

        // Act
        await _service.EvaluateMatch(resume, jobListing);

        // Assert
        _mockAiAgentService.Verify(
            s => s.MakeRequestAsync(It.Is<string>(p => p.Contains("CANDIDATE CV"))),
            Times.Once);
    }

    [Fact]
    public async Task EvaluateMatch_PropagatesAiServiceException()
    {
        // Arrange
        var resume = new UserResumeDto();
        var jobListing = "Test job";

        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("AI service error"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await _service.EvaluateMatch(resume, jobListing));
    }
}