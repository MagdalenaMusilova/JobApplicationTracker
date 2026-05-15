using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using JobApplicationTracker.Wpf.Models;

namespace JobApplicationTracker.Wpf.Services;

public class ApplicationApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5131";

    public ApplicationApiService(string? token)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        if (!string.IsNullOrWhiteSpace(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<JobApplicationMinimal>> GetApplicationsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<JobApplicationMinimal>>("/api/applications");
        return result ?? [];
    }

    public async Task<ApplicationDetail?> GetDetailAsync(Guid id) =>
        await _httpClient.GetFromJsonAsync<ApplicationDetail>($"/api/applications/{id}");

    public async Task<ApplicationDetail?> UpdateApplicationAsync(Guid id, UpdateApplicationRequest req)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/applications/{id}", req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ApplicationDetail>();
    }

    public async Task CreateApplicationAsync(CreateApplicationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/applications", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DenyApplicationAsync(string id)
    {
        var response = await _httpClient.PutAsync($"/api/applications/{id}/deny", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteApplicationAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"/api/applications/{id}");
        response.EnsureSuccessStatusCode();
    }

    // Status entries
    public async Task<ApplicationDetail?> PushStatusAsync(CreateStatusRequest req)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/applications/entry", req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ApplicationDetail>();
    }

    public async Task<StatusEntry?> UpdateStatusEntryAsync(Guid entryId, UpdateStatusRequest req)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/applications/entry/{entryId}", req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StatusEntry>();
    }

    public async Task<ApplicationDetail?> DeleteStatusEntryAsync(Guid entryId)
    {
        var response = await _httpClient.DeleteAsync($"/api/applications/entry/{entryId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ApplicationDetail>();
    }

    // Events
    public async Task<AppEvent?> CreateEventAsync(CreateAppEventRequest req)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/applications/event", req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AppEvent>();
    }

    public async Task<AppEvent?> UpdateEventAsync(Guid eventId, UpdateAppEventRequest req)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/applications/event/{eventId}", req);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AppEvent>();
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        var response = await _httpClient.DeleteAsync($"/api/applications/event/{eventId}");
        response.EnsureSuccessStatusCode();
    }
}