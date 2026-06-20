using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace Test.Integration;

public class JobApplicationIntegrationTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly IntegrationTestWebAppFactory _factory;

    public JobApplicationIntegrationTests(IntegrationTestWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateJobApplication_WithValidData_ReturnsCreated()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createRequest = new
        {
            company = "Test Company",
            position = "Software Engineer",
            note = "Test note",
            initialStatus = new
            {
                statusType = 1,
                note = "Applied"
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/applications", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<JobApplicationResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBeEmpty();
        result.Company.Should().Be("Test Company");
        result.Position.Should().Be("Software Engineer");
    }

    [Fact]
    public async Task CreateJobApplication_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var createRequest = new
        {
            company = "Test Company",
            position = "Software Engineer",
            initialStatus = new { statusType = 1 }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/applications", createRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetJobApplications_WithAuth_ReturnsApplications()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a test application first
        await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "Get Test Company",
            position = "Get Test Position",
            initialStatus = new { statusType = 1 }
        });

        // Act
        var response = await _client.GetAsync("/api/applications");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var results = await response.Content.ReadFromJsonAsync<List<JobApplicationResponse>>();
        results.Should().NotBeNull();
        results.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetJobApplicationById_WithValidId_ReturnsApplication()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "GetById Test Company",
            position = "GetById Test Position",
            initialStatus = new { statusType = 1 }
        });

        var created = await createResponse.Content.ReadFromJsonAsync<JobApplicationResponse>();

        // Act
        var response = await _client.GetAsync($"/api/applications/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<JobApplicationResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().Be(created.Id);
        result.Company.Should().Be("GetById Test Company");
    }

    [Fact]
    public async Task UpdateJobApplication_WithValidData_ReturnsOk()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "Update Test Company",
            position = "Update Test Position",
            initialStatus = new { statusType = 1 }
        });

        var created = await createResponse.Content.ReadFromJsonAsync<JobApplicationResponse>();

        var updateRequest = new
        {
            company = "Updated Company Name",
            position = "Updated Position",
            note = "Updated note"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/applications/{created!.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<JobApplicationResponse>();
        result.Should().NotBeNull();
        result!.Company.Should().Be("Updated Company Name");
        result.Position.Should().Be("Updated Position");
    }

    [Fact]
    public async Task DeleteJobApplication_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var token = await _factory.GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.PostAsJsonAsync("/api/applications", new
        {
            company = "Delete Test Company",
            position = "Delete Test Position",
            initialStatus = new { statusType = 1 }
        });

        var created = await createResponse.Content.ReadFromJsonAsync<JobApplicationResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/applications/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/applications/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private class JobApplicationResponse
    {
        public Guid Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
