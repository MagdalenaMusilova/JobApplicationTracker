using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class UserStatsService : IUserStatsService
{
    private readonly IUserStatsRepository _userStatsRepository;
    
    public UserStatsService(IUserStatsRepository userStatsRepository)
    {
        _userStatsRepository = userStatsRepository;
    }
    
    public async Task<int> CountJobApplicationsAsync()
    {
        var res = await _userStatsRepository.CountJobApplicationsAsync();
        return res;
    }
}