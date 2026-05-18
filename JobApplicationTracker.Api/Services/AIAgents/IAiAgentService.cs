namespace JobApplicationTracker.Services;

public interface IAiAgentService
{
    public Task<string> MakeRequestAsync(string prompt);
}