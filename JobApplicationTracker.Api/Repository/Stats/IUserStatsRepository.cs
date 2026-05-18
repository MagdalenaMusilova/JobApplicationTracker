namespace JobApplicationTracker.Repository;

public interface IUserStatsRepository
{
    Task<int> CountJobApplicationsAsync();
}