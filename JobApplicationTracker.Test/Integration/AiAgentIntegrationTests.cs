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

public class AiAgentIntegrationTests
{
    [Fact(Skip = "Only run manually with real API key")]
    [Trait("Category", "Integration")]
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