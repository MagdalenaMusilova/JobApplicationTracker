using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using JobApplicationTracker.Wpf.Models.Enums;

namespace JobApplicationTracker.Wpf.Services;

public class StatusApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5131";

    public StatusApiService(string? token)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        if (!string.IsNullOrWhiteSpace(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }
    
    public async Task<List<JaStatusType>> GetStatusTypesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<JaStatusType>>(
            "/api/statuses/types"
        );
        return result ?? [];
    }
}