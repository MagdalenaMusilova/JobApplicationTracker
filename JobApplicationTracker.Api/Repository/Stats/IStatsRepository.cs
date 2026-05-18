using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IStatsRepository
{
    Task<IEnumerable<User>> GetAllUsersWOnlyWishlistedAsync();
    Task<IEnumerable<JobApplication>> GetAllJAWithAllStatusesAsync();
    Task<IEnumerable<UserXResumeDto>> GetUserXResumeAsync();
    Task<IEnumerable<JAStatusEntry>> GetStatusesWEventsLeftJoinAsync();
    Task<IEnumerable<JAStatusEntry>> GetStatusesWEventsFullJoinAsync();
    Task<IEnumerable<JobApplication>> GetJAWithInterviewsOrTasksAsync();
    Task<IEnumerable<JobApplication>> GetJAWithInterviewsAndTasksAsync();
    Task<IEnumerable<JobApplication>> GetJANotWishlistedAsync();
    Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync();
}