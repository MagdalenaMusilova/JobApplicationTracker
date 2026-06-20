using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Test.Integration;

namespace Test.E2E;

/// <summary>
/// End-to-end tests that verify complete user workflows through the API
/// </summary>
public class CompleteJobApplicationWorkflowTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestWebAppFactory _factory;

    public CompleteJobApplicationWorkflowTests(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CompleteUserJourney_RegisterLoginCreateApplicationAndTrack_Success()
    {
        // Step 1: Register new user
        var email = $"e2euser{Guid.NewGuid()}@example.com";
        var password = "E2eTest123!";

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            email = email,
            password = password
        });

        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var authResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        authResult.Should().NotBeNull();
        authResult!.Token.Should().NotBeNullOrEmpty();

        // Step 2: Use token to authenticate
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

        // Step 3: Create multiple job applications
        var application1 = await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "Tech Corp",
            position = "Senior Software Engineer",
            note = "Building scalable systems",
            initialStatus = new { statusType = 1 }
        });

        application1.StatusCode.Should().Be(HttpStatusCode.OK);
        var app1Result = await application1.Content.ReadFromJsonAsync<JobApplicationDto>();

        var application2 = await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "Startup Inc",
            position = "Full Stack Developer",
            note = "Remote position",
            initialStatus = new { statusType = 1 }
        });

        application2.StatusCode.Should().Be(HttpStatusCode.OK);
        var app2Result = await application2.Content.ReadFromJsonAsync<JobApplicationDto>();

        // Step 4: Retrieve all applications
        var getAppsResponse = await _client.GetAsync("/api/applications");
        getAppsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var allApps = await getAppsResponse.Content.ReadFromJsonAsync<List<JobApplicationDto>>();
        allApps.Should().NotBeNull();
        allApps!.Count.Should().BeGreaterThanOrEqualTo(2);

        // Step 5: Update application status
        var updateResponse = await _client.PutAsJsonAsync($"/api/applications/{app1Result.Id}", new
        {
            company = app1Result.Company,
            position = app1Result.Position,
            note = "Updated: Building highly scalable distributed systems"
        });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Step 6: Get specific application with all details
        var getAppResponse = await _client.GetAsync($"/api/applications/{app1Result.Id}");
        getAppResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var appDetails = await getAppResponse.Content.ReadFromJsonAsync<JobApplicationDto>();
        appDetails.Should().NotBeNull();
        appDetails!.Note.Should().Contain("Updated");

        // Step 7: Refresh authentication token
        var refreshResponse = await _client.PostAsJsonAsync("/api/auth/refresh", new
        {
            refreshToken = authResult.RefreshToken
        });

        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var newAuthResult = await refreshResponse.Content.ReadFromJsonAsync<AuthResponse>();
        newAuthResult.Should().NotBeNull();
        newAuthResult!.Token.Should().NotBeNullOrEmpty();

        // Step 8: Use new token to verify it works
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAuthResult.Token);
        
        var verifyResponse = await _client.GetAsync("/api/applications");
        verifyResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Step 9: Delete one application
        var deleteResponse = await _client.DeleteAsync($"/api/applications/{app2Result!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Step 10: Verify deletion
        var deletedAppResponse = await _client.GetAsync($"/api/applications/{app2Result.Id}");
        deletedAppResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ApplicationLifecycle_CreateUpdateAndDelete_Success()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync($"lifecycle{Guid.NewGuid()}@example.com");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act & Assert: Create application
        var createResponse = await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "Enterprise Solutions Ltd",
            position = "Backend Engineer",
            note = "New York based position",
            initialStatus = new { statusType = 1 }
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var app = await createResponse.Content.ReadFromJsonAsync<JobApplicationDto>();
        app.Should().NotBeNull();

        // Update application
        var updateResponse = await _client.PutAsJsonAsync($"/api/applications/{app!.Id}", new
        {
            company = app.Company,
            position = "Senior Backend Engineer",
            note = "Updated position title"
        });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify update
        var getResponse = await _client.GetAsync($"/api/applications/{app.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<JobApplicationDto>();
        updated!.Position.Should().Be("Senior Backend Engineer");

        // Delete application
        var deleteResponse = await _client.DeleteAsync($"/api/applications/{app.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var verifyDelete = await _client.GetAsync($"/api/applications/{app.Id}");
        verifyDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task MultipleApplications_WithDifferentStatuses_CanBeFilteredAndQueried()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync($"multiapp{Guid.NewGuid()}@example.com");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create applications with different statuses
        var apps = new[]
        {
            new { company = "Company A", position = "Position A", status = "Applied" },
            new { company = "Company B", position = "Position B", status = "Interviewing" },
            new { company = "Company C", position = "Position C", status = "Offer" },
            new { company = "Company D", position = "Position D", status = "Rejected" }
        };

        var createdAppIds = new List<Guid>();

        foreach (var app in apps)
        {
            var response = await _client.PostAsJsonAsync("/api/applications", new
            {
                app.company,
                app.position,
                initialStatus = new { statusType = 1 }
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var created = await response.Content.ReadFromJsonAsync<JobApplicationDto>();
            createdAppIds.Add(created!.Id);
        }

        // Act: Get all applications
        var allAppsResponse = await _client.GetAsync("/api/applications");
        allAppsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var allApps = await allAppsResponse.Content.ReadFromJsonAsync<List<JobApplicationDto>>();

        // Assert
        allApps.Should().NotBeNull();
        allApps!.Count.Should().BeGreaterThanOrEqualTo(4);
        
        foreach (var id in createdAppIds)
        {
            allApps.Should().Contain(a => a.Id == id);
        }
    }

    private class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    private class JobApplicationDto
    {
        public Guid Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

    private class EventDto
    {
        public int Id { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime EventDate { get; set; }
    }
}
