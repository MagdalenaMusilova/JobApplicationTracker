using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Test.Integration;

public class AiAgentIntegrationTests
{
    [Fact(Skip = "Only run manually with real API key")]
    public async Task MakeRequestAsync_RealOpenAI_ReturnsValidResponse()
    {
        // Arrange
        var httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    
        var service = new OpenAiAgentService(httpClient, configuration);
        var prompt = "Say 'test' and nothing else";

        // Act
        var result = await service.MakeRequestAsync(prompt);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("test");
    }
}