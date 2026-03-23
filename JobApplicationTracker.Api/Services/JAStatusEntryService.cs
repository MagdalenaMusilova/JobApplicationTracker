using AutoMapper;
using JobApplicationTracker.Dos;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class JAStatusEntryService : IJAStatusEntryService
{
    private readonly IJAStatusEntryRepository _jaStatusEntryRepository;
    private readonly IMapper _mapper;
    
    public JAStatusEntryService(IJAStatusEntryRepository jaStatusEntryRepository, IMapper mapper)
    {
        _jaStatusEntryRepository = jaStatusEntryRepository;
    }
    
    public async Task<JAStatusEntryDto> AddAsync(JobApplicationDto jobApplication, CreateJAStatusEntryDto jaStatusEntry)
    {
        int statusEntriesCount = jobApplication.StatusHistory.Count();
        
        DateTime now = DateTime.UtcNow;
        var jaStatusDo = new JAStatusEntryDo
        {
            JobApplicationId = jaStatusEntry.JobApplicationId,
            OrderIndex = statusEntriesCount,
            JaStatus = jaStatusEntry.JaStatus,
            CreatedAt = now,
            UpdatedAt = now,
            Note = jaStatusEntry.Note
        };
        
        var createdEntry = await _jaStatusEntryRepository.AddAsync(jaStatusDo);
        
        return _mapper.Map<JAStatusEntryDto>(createdEntry);   
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<int> ids)
    {
        return await _jaStatusEntryRepository.DeleteBulkAsync(ids);
    }
}