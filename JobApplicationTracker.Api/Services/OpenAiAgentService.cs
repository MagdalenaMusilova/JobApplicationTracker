using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JobApplicationTracker.Services;

public class OpenAiAgentService : IAiAgentService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiAgentService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key is missing.");
    }
    
    public async Task<string> MakeRequestAsync(string prompt)
    {
        var requestBody = new
        {
            model = "gpt-4.1-mini",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        using var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"OpenAI request failed with status {(int)response.StatusCode}: {responseContent}");
        }

        using var doc = JsonDocument.Parse(responseContent);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;
    }
}