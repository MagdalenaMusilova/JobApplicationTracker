using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IStatsService
{
    Task<IEnumerable<UserDto>> GetAllUsersWOnlyWhishlistedAsync();
    Task<IEnumerable<JobApplicationDto>> GetAllJAWithAllStatusesAsync();
    Task<IEnumerable<ProfileXResumeDto>> GetStatusXEventAsync();
    Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsLeftJoinAsync();
    Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsFullJoinAsync();
    Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsOrTasksAsync();
    Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsAndTasksAsync();
    Task<IEnumerable<JobApplicationDto>> GetJANotWhishlistedAsync();
    Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync();
}