using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JAEventRepository : IJAEventRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public JAEventRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JAEventDo?> GetByIdAsync(Guid id)
    {
        return await _context.JAEventEntries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => _mapper.Map<JAEventDo>(e))
            .FirstOrDefaultAsync();
    }

    public async Task<List<JAEventDo>> GetByStatusIdsAsync(IEnumerable<Guid> statusIds)
    {
        return await _context.JAEventEntries
            .AsNoTracking()
            .Where(e => statusIds.Contains(e.Id))
            .Select(e => _mapper.Map<JAEventDo>(e))
            .ToListAsync();
    }

    public async Task<JAEventDo> AddAsync(JAEventDo jaEvent)
    {
        var entity = _mapper.Map<JAEvent>(jaEvent);
        _context.JAEventEntries.Add(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<JAEventDo>(entity);  
    }

    public async Task<JAEventDo?> UpdateAsync(JAEventDo updated)
    {
        var entity = await _context.JAEventEntries.FindAsync(updated.Id);
        if (entity is null) return null;
        
        entity.EventName = updated.EventName;
        entity.EventType = updated.EventType;
        entity.EventDate = updated.EventDate;
        entity.IsWholeDay = updated.IsWholeDay;
        entity.Note = updated.Note;
        
        await _context.SaveChangesAsync();
        
        return _mapper.Map<JAEventDo>(entity);  
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids)
    {
        await _context.JAEventEntries
            .Where(e => ids.Contains(e.Id))
            .ExecuteDeleteAsync<JAEvent>();
        return true;
    }
}