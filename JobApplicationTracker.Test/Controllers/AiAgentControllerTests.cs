using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test.Controllers;

public class AiAgentControllerTests
{
    private readonly Mock<IAiAgentService> _mockAiAgentService;
    private readonly AiAgentController _controller;

    public AiAgentControllerTests()
    {
        _mockAiAgentService = new Mock<IAiAgentService>();
        _controller = new AiAgentController(_mockAiAgentService.Object);
    }

    [Fact]
    public async Task MakeRequest_ReturnsOkWithResponse()
    {
        // Arrange
        var prompt = "Hello AI";
        var expectedResponse = "Hello! How can I help you?";
        
        _mockAiAgentService
            .Setup(s => s.MakeRequestAsync(prompt))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.MakeRequest(prompt);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(expectedResponse);
        _mockAiAgentService.Verify(s => s.MakeRequestAsync(prompt), Times.Once);
    }

    [Fact]
    public async Task MakeRequest_CallsServiceWithCorrectPrompt()
    {
        // Arrange
        var prompt = "Test prompt";
        _mockAiAgentService.Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ReturnsAsync("response");

        // Act
        await _controller.MakeRequest(prompt);

        // Assert
        _mockAiAgentService.Verify(
            s => s.MakeRequestAsync(prompt), 
            Times.Once);
    }
}