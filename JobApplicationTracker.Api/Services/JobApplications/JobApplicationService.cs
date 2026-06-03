using AutoMapper;
using AutoMapper.QueryableExtensions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _applicationRepository;
    private readonly IJAStatusEntryService _jaStatusEntryService;
    private readonly IMapper _mapper;

    public JobApplicationService(IJobApplicationRepository applicationRepository, IJAStatusEntryService jaStatusEntryService, IMapper mapper)
    {
        _applicationRepository = applicationRepository;
        _jaStatusEntryService = jaStatusEntryService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<JobApplicationDto>> GetAllAsync()
    {
        var applications = await _applicationRepository.GetAllAsync();
        return applications.Select(a => _mapper.Map<JobApplicationDto>(a));    
    }

    public async Task<IEnumerable<JobApplicationDto>> GetAllByUserAsync(string userId)
    {
        var applications = await _applicationRepository.GetAllByUserAsync(userId);
        return applications.Select(a => _mapper.Map<JobApplicationDto>(a));
    }

    public async Task<IEnumerable<JobApplicationMinimalDto>> GetAllByUserMinimalAsync(string userId)
    {
        var minApplications = await _applicationRepository.GetAllByUserMinimalAsync(userId);
        return minApplications.Select(ja => _mapper.Map<JobApplicationMinimalDto>(ja));
    }

    public async Task<IEnumerable<JobApplicationMinimalDto>> GetAllNotFinishedAsync(string userId)
    {
        var applications = await _applicationRepository.GetAllNotFinishedAsync(userId);
        return applications.Select(a => _mapper.Map<JobApplicationMinimalDto>(a));
    }

    public async Task<JobApplicationDto?> GetByIdAsync(Guid id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        return application is null ? null : _mapper.Map<JobApplicationDto>(application);
    }

    public async Task<JobApplicationDto> AddAsync(string userId, CreateJobApplicationDto application)
    {
        var entity = new JobApplication
        {
            UserId = userId,
            Company = application.Company,
            Position = application.Position,
            Note = application.Note,
            StatusHistory = []
        };

        var created = await _applicationRepository.AddAsync(entity);

        var createStatusEntry = new CreateJAStatusEntryDto
        {
            JobApplicationId = created.Id,
            StatusType = application.InitialStatus.StatusType,
            Note = application.InitialStatus.Note
        };

        var createdDto = _mapper.Map<JobApplicationDto>(created);
        await _jaStatusEntryService.AddAsync(createdDto, createStatusEntry);

        return await GetByIdAsync(created.Id);
    }

    public async Task<JobApplicationDto?> UpdateAsync(Guid id, UpdateJobApplicationDto application)
    {
        var existing = await _applicationRepository.GetByIdAsync(id);
        if (existing is null) return null;

        var entity = new JobApplication
        {
            Id = existing.Id,
            UserId = existing.UserId,
            Company = application.Company ?? existing.Company,
            Position = application.Position ?? existing.Position,
            Note = application.Note ?? existing.Note,
            StatusHistory = existing.StatusHistory
        };

        var updated = await _applicationRepository.UpdateAsync(entity);
        return _mapper.Map<JobApplicationDto>(updated);
    }

    public Task<bool> DeleteAsync(Guid id) => _applicationRepository.DeleteAsync(id);

    public async Task<JobApplicationDto> PushApplicationStatusAsync(CreateJAStatusEntryDto statusEntry)
    {
        var application = await GetByIdAsync(statusEntry.JobApplicationId)
                          ?? throw new InvalidOperationException("Application not found.");

        await _jaStatusEntryService.AddAsync(application, statusEntry);

        var updated = await _applicationRepository.GetByIdAsync(application.Id);
        return _mapper.Map<JobApplicationDto>(updated);
    }
    
}