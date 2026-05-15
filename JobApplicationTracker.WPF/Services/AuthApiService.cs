using System.Net.Http;
using System.Net.Http.Json;

namespace JobApplicationTracker.Wpf.Services;

public class AuthApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:5131";

    public AuthApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    public async Task SignUpAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/signup", new { username, password });
        if (!response.IsSuccessStatusCode)
            throw new Exception("Sign up failed. Try a different username.");
    }

    public async Task<string?> SignInAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/signin", new { username, password });
        if (!response.IsSuccessStatusCode)
            return null;

        var result = await response.Content.ReadFromJsonAsync<SignInResponse>();
        return result?.Token;
    }

    private record SignInResponse(string Token);
}