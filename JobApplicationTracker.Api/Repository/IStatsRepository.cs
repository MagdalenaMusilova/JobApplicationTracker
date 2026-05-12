using JobApplicationTracker.Models;
using JobApplicationTracker.Models.Stats;

namespace JobApplicationTracker.Repository;

public interface IStatsRepository
{
    Task<IEnumerable<User>> GetAllUsersWOnlyWhishlistedAsync();
    Task<IEnumerable<JobApplication>> GetAllJAWithAllStatusesAsync();
    Task<IEnumerable<ProfileXResumeDto>> GetStatusXEventAsync();
    Task<IEnumerable<JAStatusEntry>> GetStatusesWEventsLeftJoinAsync();
    Task<IEnumerable<JAStatusEntry>> GetStatusesWEventsFullJoinAsync();
    Task<IEnumerable<JobApplication>> GetJAWithInterviewsOrTasksAsync();
    Task<IEnumerable<JobApplication>> GetJAWithInterviewsAndTasksAsync();
    Task<IEnumerable<JobApplication>> GetJANotWhishlistedAsync();
    Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync();
}