using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Moq;

namespace Test.Services;

public class JobListingServiceTests
{
    private readonly Mock<IJobListingExtractor> _mockExtractor;
    private readonly JobListingService _service;

    public JobListingServiceTests()
    {
        _mockExtractor = new Mock<IJobListingExtractor>();
        _service = new JobListingService(_mockExtractor.Object);
    }

    [Fact]
    public async Task ExtractFromPlaintext_ReturnsJobListing_WhenSuccessful()
    {
        // Arrange
        var text = "Software Developer position at Tech Corp...";
        var expectedDto = new JobListingDto
        {
            JobDescription = "Software Developer position",
        };

        _mockExtractor.Setup(e => e.ExtractFromPlaintextAsync(text)).ReturnsAsync(expectedDto);

        // Act
        var result = await _service.ExtractFromPlaintext(text);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedDto);
        _mockExtractor.Verify(e => e.ExtractFromPlaintextAsync(text), Times.Once);
    }

    [Fact]
    public async Task ExtractFromPlaintext_ReturnsNull_WhenExtractionFails()
    {
        // Arrange
        var text = "Invalid text";
        _mockExtractor.Setup(e => e.ExtractFromPlaintextAsync(text)).ReturnsAsync((JobListingDto?)null);

        // Act
        var result = await _service.ExtractFromPlaintext(text);

        // Assert
        result.Should().BeNull();
        _mockExtractor.Verify(e => e.ExtractFromPlaintextAsync(text), Times.Once);
    }
}