using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public interface IJAEventService
{
    Task<JAEventDto?> GetByIdAsync(Guid id);
    Task<List<JAEventDto>> GetAllByUserId(Guid userId);
    Task<List<JAEventDto>> GetByStatusIdsAsync(IEnumerable<Guid> statusIds);
    Task<JAEventDto> AddAsync(CreateJAEventDto jaEvent);
    Task<JAEventDto?> UpdateAsync(Guid id, UpdateJAEventDto updated);
    Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids);
}