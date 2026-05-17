using System.Net;
using System.Text.Json;
using FluentAssertions;
using JobApplicationTracker.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace Test.Services;

public class OpenAiAgentServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly HttpClient _httpClient;
    private readonly OpenAiAgentService _service;

    public OpenAiAgentServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _mockConfiguration = new Mock<IConfiguration>();
        
        _mockConfiguration.Setup(c => c["OpenAI:ApiKey"]).Returns("test-api-key");
        _service = new OpenAiAgentService(_httpClient, _mockConfiguration.Object);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenApiKeyIsMissing()
    {
        // Arrange
        var config = new Mock<IConfiguration>();
        config.Setup(c => c["OpenAI:ApiKey"]).Returns((string?)null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => new OpenAiAgentService(_httpClient, config.Object));
        
        exception.Message.Should().Be("OpenAI API key is missing.");
    }

    [Fact]
    public async Task MakeRequestAsync_ReturnsContent_WhenSuccessful()
    {
        // Arrange
        var prompt = "Hello AI";
        var responseJson = JsonSerializer.Serialize(new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content = "Hello! How can I help you?"
                    }
                }
            }
        });

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            });

        // Act
        var result = await _service.MakeRequestAsync(prompt);

        // Assert
        result.Should().Be("Hello! How can I help you?");
    }


    [Fact]
    public async Task MakeRequestAsync_SendsCorrectRequest()
    {
        // Arrange
        var prompt = "Test prompt";
        HttpRequestMessage? capturedRequest = null;
        string? capturedContent = null;

        var responseJson = JsonSerializer.Serialize(new
        {
            choices = new[]
            {
                new { message = new { content = "response" } }
            }
        });

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>(async (req, _) =>
            {
                capturedRequest = req;
                // Read content immediately before it gets disposed
                capturedContent = await req.Content!.ReadAsStringAsync();
            })
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            });

        // Act
        await _service.MakeRequestAsync(prompt);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri.Should().Be("https://api.openai.com/v1/chat/completions");
        capturedRequest.Headers.Authorization.Should().NotBeNull();
        capturedRequest.Headers.Authorization!.Scheme.Should().Be("Bearer");
        capturedRequest.Headers.Authorization.Parameter.Should().Be("test-api-key");

        capturedContent.Should().NotBeNull();
        capturedContent!.Should().Contain(prompt);
        capturedContent.Should().Contain("gpt-4.1-mini");
    }

    [Fact]
    public async Task MakeRequestAsync_ThrowsHttpRequestException_WhenRequestFails()
    {
        // Arrange
        var prompt = "Test";
        
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Invalid request")
            });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            async () => await _service.MakeRequestAsync(prompt));
        
        exception.Message.Should().Contain("400");
        exception.Message.Should().Contain("Invalid request");
    }

    [Fact]
    public async Task MakeRequestAsync_ReturnsEmptyString_WhenContentIsNull()
    {
        // Arrange
        var prompt = "Test";
        var responseJson = JsonSerializer.Serialize(new
        {
            choices = new[]
            {
                new { message = new { content = (string?)null } }
            }
        });

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            });

        // Act
        var result = await _service.MakeRequestAsync(prompt);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public async Task MakeRequestAsync_HandlesUnauthorizedError()
    {
        // Arrange
        var prompt = "Test";
        
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("Invalid API key")
            });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            async () => await _service.MakeRequestAsync(prompt));
        
        exception.Message.Should().Contain("401");
    }
}