using AutoMapper;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class StatsService : IStatsService
{
    private IStatsRepository _statsRepository;
    private IMapper _mapper;
    
    public StatsService(IStatsRepository statsRepository, IMapper mapper)
    {
        _statsRepository = statsRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersWOnlyWhishlistedAsync()
    {
        var res = await _statsRepository.GetAllUsersWOnlyWhishlistedAsync();
        return res.Select(u => _mapper.Map<UserDto>(u));
    }

    public async Task<IEnumerable<JobApplicationDto>> GetAllJAWithAllStatusesAsync()
    {
        var res = await _statsRepository.GetAllJAWithAllStatusesAsync();
        return res.Select(ja => _mapper.Map<JobApplicationDto>(ja));
    }

    public async Task<IEnumerable<ProfileXResumeDto>> GetStatusXEventAsync()
    {
        var res = await _statsRepository.GetStatusXEventAsync();
        return res;
    }

    public async Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsLeftJoinAsync()
    {
        var res = await _statsRepository.GetStatusesWEventsLeftJoinAsync();
        return res.Select(stat => _mapper.Map<JAStatusEntryDto>(stat));
    }

    public async Task<IEnumerable<JAStatusEntryDto>> GetStatusesWEventsFullJoinAsync()
    {
        var res = await _statsRepository.GetStatusesWEventsFullJoinAsync();
        return res.Select(stat => _mapper.Map<JAStatusEntryDto>(stat));
    }

    public async Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsOrTasksAsync()
    {
        var res = await _statsRepository.GetJAWithInterviewsOrTasksAsync();
        return res.Select(ja => _mapper.Map<JobApplicationDto>(ja));
    }

    public async Task<IEnumerable<JobApplicationDto>> GetJAWithInterviewsAndTasksAsync()
    {
        var res = await _statsRepository.GetJAWithInterviewsAndTasksAsync();
        return res.Select(ja => _mapper.Map<JobApplicationDto>(ja));    
    }

    public async Task<IEnumerable<JobApplicationDto>> GetJANotWhishlistedAsync()
    {
        var res = await _statsRepository.GetJANotWhishlistedAsync();
        return res.Select(ja => _mapper.Map<JobApplicationDto>(ja));    
    }

    public async Task<List<ApplicationCountDto>> JobApplicationsWithMoreThanOneStatusAsync()
    {
        var res = await _statsRepository.JobApplicationsWithMoreThanOneStatusAsync();
        return res;
    }
}