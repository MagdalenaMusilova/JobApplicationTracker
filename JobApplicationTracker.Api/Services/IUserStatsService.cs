namespace JobApplicationTracker.Services;

public interface IUserStatsService
{
    Task<int> CountJobApplicationsAsync();
}       