using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IMockDataService
{
    Task FillAccountWithMockDataAsync(string userId);
}
