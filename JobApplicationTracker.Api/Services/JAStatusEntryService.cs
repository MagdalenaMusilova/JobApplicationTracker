using AutoMapper;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class JAStatusEntryService : IJAStatusEntryService
{
    private readonly IJAStatusEntryRepository _jaStatusEntryRepository;
    private readonly IMapper _mapper;
    
    public JAStatusEntryService(IJAStatusEntryRepository jaStatusEntryRepository, IMapper mapper)
    {
        _jaStatusEntryRepository = jaStatusEntryRepository;
        _mapper = mapper;   
    }

    public async Task<JAStatusEntryDto?> GetByIdAsync(Guid id)
    {
        var entry = await _jaStatusEntryRepository.GetByIdAsync(id);
        if (entry is null) return null;
        return _mapper.Map<JAStatusEntryDto>(entry);   
    }

    public async Task<List<JAStatusEntryDto>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationId)
    {
        var res = await _jaStatusEntryRepository.GetByJobApplicationIdsAsync(jobApplicationId);
        return res.Select(e => _mapper.Map<JAStatusEntryDto>(e)).ToList();  
    }

    public async Task<JAStatusEntryDto> AddAsync(JobApplicationDto jobApplication, CreateJAStatusEntryDto jaStatusEntry)
    {
        int statusEntriesCount = jobApplication.StatusHistory.Count();
        
        DateTime now = DateTime.UtcNow;
        var entity = new JAStatusEntry
        {
            JobApplicationId = jaStatusEntry.JobApplicationId,
            OrderIndex = statusEntriesCount,
            JaStatusType = (JAStatusType)jaStatusEntry.StatusType,
            CreatedAt = now,
            Note = jaStatusEntry.Note
        };
        
        var createdEntry = await _jaStatusEntryRepository.AddAsync(entity);
        return _mapper.Map<JAStatusEntryDto>(createdEntry);   
    }

    public async Task<JAStatusEntryDto?> UpdateAsync(Guid id, CreateJAStatusEntryDto updated)
    {
        var existing = await _jaStatusEntryRepository.GetByIdAsync(id);
        if (existing is null) return null;

        var entity = new JAStatusEntry
        {
            JaStatusType = (JAStatusType)updated.StatusType,
            Note = updated.Note,
        };

        var result = await _jaStatusEntryRepository.UpdateAsync(id, entity);
        return result is null ? null : _mapper.Map<JAStatusEntryDto>(result);
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids)
    {
        return await _jaStatusEntryRepository.DeleteBulkAsync(ids);
    }
}