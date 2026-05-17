using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class AiJobListingExtractorTests
{
    private readonly Mock<IAiAgentService> _mockAiAgentService;
    private readonly AiJobListingExtractor _extractor;

    public AiJobListingExtractorTests()
    {
        _mockAiAgentService = new Mock<IAiAgentService>();
        _extractor = new AiJobListingExtractor(_mockAiAgentService.Object);
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_ReturnsJobListingDto_WithJobDescription()
    {
        // Arrange
        var jobText = "Senior Software Engineer position requiring 5+ years experience in C# and .NET";

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(jobText);

        // Assert
        result.Should().NotBeNull();
        result!.JobDescription.Should().Be(jobText);
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_HandlesEmptyString()
    {
        // Arrange
        var jobText = string.Empty;

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(jobText);

        // Assert
        result.Should().NotBeNull();
        result!.JobDescription.Should().BeEmpty();
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_PreservesOriginalText()
    {
        // Arrange
        var jobText = @"We are looking for a talented developer.
            
            Requirements:
            - 3+ years of experience
            - Strong communication skills
            - Bachelor's degree preferred";

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(jobText);

        // Assert
        result.Should().NotBeNull();
        result!.JobDescription.Should().Be(jobText);
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_DoesNotCallAiService()
    {
        // Arrange
        var jobText = "Test job listing";

        // Act
        await _extractor.ExtractFromPlaintextAsync(jobText);

        // Assert
        _mockAiAgentService.Verify(
            s => s.MakeRequestAsync(It.IsAny<string>()),
            Times.Never,
            "ExtractFromPlaintextAsync should not call AI service");
    }

    [Fact]
    public async Task ExtractFromPlaintextAsync_HandlesLongJobDescription()
    {
        // Arrange
        var jobText = new string('x', 10000); // Very long text

        // Act
        var result = await _extractor.ExtractFromPlaintextAsync(jobText);

        // Assert
        result.Should().NotBeNull();
        result!.JobDescription.Should().HaveLength(10000);
    }
}