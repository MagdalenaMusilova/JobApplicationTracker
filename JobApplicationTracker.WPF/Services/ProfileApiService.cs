using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using JobApplicationTracker.Wpf.Models;
using JobApplicationTracker.Wpf.Models.Profile;

namespace JobApplicationTracker.Wpf.Services;

public class ProfileApiService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "http://localhost:5131";

    public ProfileApiService(string? token)
    {
        _http = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        if (!string.IsNullOrWhiteSpace(token))
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<UserProfileDto?> GetMeAsync() =>
        await _http.GetFromJsonAsync<UserProfileDto>("/api/users");

    public async Task<UserResumeDto?> GetResumeByUserIdAsync(string userId) =>
        await _http.GetFromJsonAsync<UserResumeDto>($"/api/resume/user={userId}");
}