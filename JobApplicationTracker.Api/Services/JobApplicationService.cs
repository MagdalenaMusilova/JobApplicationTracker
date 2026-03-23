using AutoMapper;
using JobApplicationTracker.Dos;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Repository;

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

    public async Task<IEnumerable<JobApplicationDto>> GetAllByUserAsync(int userId)
    {
        var applications = await _applicationRepository.GetAllByUserAsync(userId);

        return applications.Select(a => _mapper.Map<JobApplicationDto>(a));
    }

    public async Task<JobApplicationDto?> GetByIdAsync(int id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        if (application is null)
        {
            return null;
        }
        return _mapper.Map<JobApplicationDto>(application);
    }

    public async Task<JobApplicationDto> AddAsync(CreateJobApplicationDto application)
    {
        var applicationDo = new JobApplicationDo
        {
            UserId = application.UserId,
            Company = application.Company,
            Position = application.Position,
            StatusHistory = new List<JAStatusEntryDo>(){},
            Note = application.Note
        };
        
        var createdApplication = await _applicationRepository.AddAsync(applicationDo);
        var createdApplicationDto = _mapper.Map<JobApplicationDto>(createdApplication);
        
        var createJAStatusEntryDto = new CreateJAStatusEntryDto
        {
            JobApplicationId = createdApplication.Id,
            JaStatus = application.JaStatus,
            Note = application.JaStatusNote
        };
        var initStatus = await _jaStatusEntryService.AddAsync(createdApplicationDto, createJAStatusEntryDto);

        var updatedApplication = await GetByIdAsync(createdApplication.Id);
        return updatedApplication;
    }

    public async Task<JobApplicationDto?> UpdateAsync(int id, UpdateJobApplicationDto application)
    {
        var existingApplication = await _applicationRepository.GetByIdAsync(id);
        
        if (existingApplication == null)
        {
            return null;
        }
        
        var applicationDo = new JobApplicationDo
        {
            Id = existingApplication.Id,
            UserId = existingApplication.UserId,
            Company = application.Company ?? existingApplication.Company,
            Position = application.Position ?? existingApplication.Position,
            StatusHistory = existingApplication.StatusHistory.Select(e => _mapper.Map<JAStatusEntryDo>(e)),
            Note = application.Note ?? existingApplication.Note
        };
        
        var updatedApplication = await _applicationRepository.UpdateAsync(applicationDo);
        return _mapper.Map<JobApplicationDto>(updatedApplication);    
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _applicationRepository.DeleteAsync(id);   
    }

    public async Task<JobApplicationDto> PushApplicationStatusAsync(CreateJAStatusEntryDto statusEntry)
    {
        var application = await GetByIdAsync(statusEntry.JobApplicationId);
        if (application == null)
        {
            throw new InvalidOperationException("Application not found.");
        }
        
        var createdStatusEntry = await _jaStatusEntryService.AddAsync(application, statusEntry);
        var updatedApplication = await _applicationRepository.GetByIdAsync(application.Id);
        return _mapper.Map<JobApplicationDto>(updatedApplication);
    }

    public async Task<JobApplicationDto> PopApplicationStatusAsync(int applicationId, int lastEntryId)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId);
        if (application == null)
        {
            throw new InvalidOperationException("Application not found.");
        }
        
        var lastStatusEntry = application.StatusHistory.LastOrDefault(e => e.Id == lastEntryId);
        if (lastStatusEntry == null)
        {
            throw new InvalidOperationException("Entry not found.");
        }
        
        var entriesToDel = application.StatusHistory.Where(e => e.OrderIndex > lastStatusEntry.OrderIndex);
        var entryIdsToDel = entriesToDel.Select(e => e.Id);
        
        var success = await _jaStatusEntryService.DeleteBulkAsync(entryIdsToDel);
        if (!success)
        {
            throw new InvalidOperationException("Failed to delete status entries.");
        }
        
        var updatedApplication = await _applicationRepository.GetByIdAsync(applicationId);
        return _mapper.Map<JobApplicationDto>(updatedApplication);   
    }
}