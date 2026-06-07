using AutoMapper;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class JAEventService : IJAEventService
{
    private readonly IJAEventRepository _jaEventRepository;
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IJAStatusEntryService _jaStatusEntryService;
    private readonly IMapper _mapper;
    
    public JAEventService(IJAEventRepository jaEventRepository, IJobApplicationService jobApplicationService, IJAStatusEntryService jaStatusEntryService, IMapper mapper)
    {
        _jaEventRepository = jaEventRepository;
        _jobApplicationService = jobApplicationService;
        _jaStatusEntryService = jaStatusEntryService;
        _mapper = mapper;
    }

    public async Task<JAEventDto?> GetByIdAsync(Guid id)
    {
        var res = await _jaEventRepository.GetByIdAsync(id);
        
        return _mapper.Map<JAEventDto>(res);
    }

    public async Task<List<JAEventDto>> GetAllByUserId(string userId)
    {
        var res = await _jaEventRepository.GetByUserIdAsync(userId);
        
        return res.Select(e => _mapper.Map<JAEventDto>(e)).ToList();
    }

    public async Task<List<JAEventDto>> GetByStatusIdsAsync(IEnumerable<Guid> statusIds)
    {
        var res = await _jaEventRepository.GetByStatusIdsAsync(statusIds);
        
        return res.Select(e => _mapper.Map<JAEventDto>(e)).ToList();
    }

    public async Task<JAEventDto> AddAsync(CreateJAEventDto jaEvent)
    {
        var existingEvent = await _jaEventRepository.GetByStatusIdsAsync([jaEvent.JAStatusEntryId]);
        if (existingEvent.Any()) throw new InvalidOperationException("Event already exists for this status entry");
        
        var entity = new JAEvent()
        {
            JAStatusEntryId = jaEvent.JAStatusEntryId,
            EventName = jaEvent.EventName,
            EventType = (JAEventType)jaEvent.EventType,
            EventDate = DateTime.Parse(jaEvent.EventDate),
            IsWholeDay = jaEvent.IsWholeDay,
            Note = jaEvent.Note
        };

        var created = await _jaEventRepository.AddAsync(entity);
        
        return _mapper.Map<JAEventDto>(created);
    }

    public async Task<JAEventDto?> UpdateAsync(Guid id, UpdateJAEventDto updated)
    {
        var existing = await _jaEventRepository.GetByIdAsync(id);
        if (existing is null) return null;

        var entity = new JAEvent()
        {
            Id = existing.Id,
            JAStatusEntryId = existing.JAStatusEntryId,
            EventName = updated.EventName ?? existing.EventName,
            EventType = updated.EventType ?? existing.EventType,
            EventDate = updated.EventDate ?? existing.EventDate,
            IsWholeDay = updated.IsWholeDay ?? existing.IsWholeDay,
            Note = updated.Note ?? existing.Note,
        };
        
        var updatedDo = await _jaEventRepository.UpdateAsync(entity);
        return _mapper.Map<JAEventDto>(updatedDo);
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids)
    {
        return await _jaEventRepository.DeleteBulkAsync(ids);
    }
}