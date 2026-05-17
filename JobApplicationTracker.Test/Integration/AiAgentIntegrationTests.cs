using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Test.Integration;

public class AiAgentIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AiAgentIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task MakeRequest_ReturnsOk_WithMockedService()
    {
        // Arrange
        var prompt = "Hello AI";
        var expectedResponse = "Hello! How can I help you?";
        
        _factory.MockAiAgentService
            .Setup(s => s.MakeRequestAsync(prompt))
            .ReturnsAsync(expectedResponse);

        // Act
        var response = await _client.PostAsJsonAsync("/api/ai", prompt);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain(expectedResponse);
    }

    [Fact]
    public async Task MakeRequest_Returns500_WhenServiceThrows()
    {
        // Arrange
        var prompt = "Test error";
        
        _factory.MockAiAgentService
            .Setup(s => s.MakeRequestAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("OpenAI API error"));

        // Act
        var response = await _client.PostAsJsonAsync("/api/ai", prompt);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
    
    [Fact(Skip = "Only run manually with real API key")]
    [Trait("Category", "Integration")]
    [Trait("Category", "RequiresApiKey")]
    public async Task MakeRequest_WithRealOpenAI_Works()
    {
        // Arrange - Use a factory without mocking
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
    
        var prompt = "Say 'Hello Test' and nothing else";

        // Act
        var response = await client.PostAsJsonAsync("/api/ai", prompt);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNullOrEmpty();
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IAiAgentService> MockAiAgentService { get; }

    public CustomWebApplicationFactory()
    {
        MockAiAgentService = new Mock<IAiAgentService>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real IAiAgentService registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IAiAgentService));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add the mocked service
            services.AddScoped(_ => MockAiAgentService.Object);
        });
    }
}