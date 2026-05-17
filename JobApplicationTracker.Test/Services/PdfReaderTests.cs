using FluentAssertions;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Test.Services;

public class PdfReaderTests
{
    private readonly PdfReader _service;

    public PdfReaderTests()
    {
        _service = new PdfReader();
    }

    [Fact]
    public void ReadText_ReturnsEmptyString_WhenFileIsNull()
    {
        // Act
        var result = _service.ReadText(null!);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ReadText_ReturnsEmptyString_WhenFileIsEmpty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);

        // Act
        var result = _service.ReadText(mockFile.Object);

        // Assert
        result.Should().BeEmpty();
    }
}