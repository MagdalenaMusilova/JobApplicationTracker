using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IStatsService
{
    Task<IEnumerable<UserDto>> GetAllUsersWOnlyWishlistedAsync();
    Task<IEnumerable<JobApplicationDto>> GetAllJAWithAllStatusesAsync();
    Task<IEnumerable<UserXResumeDto>> GetUserXResumeAsync();
    Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsLeftJoinAsync();
    Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsFullJoinAsync();
    Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsOrTasksAsync();
    Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsAndTasksAsync();
    Task<IEnumerable<JobApplicationDto>> GetJANotWishlistedAsync();
    Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync();
}