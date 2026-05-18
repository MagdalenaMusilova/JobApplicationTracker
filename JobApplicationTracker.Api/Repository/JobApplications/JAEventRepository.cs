using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JAEventRepository : IJAEventRepository
{
    private readonly AppDbContext _context;
    
    public JAEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JAEvent?> GetByIdAsync(Guid id)
    {
        return await _context.JAEventEntries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(e => e.JAStatusEntry)
            .FirstOrDefaultAsync();
    }

    public async Task<List<JAEvent>> GetByStatusIdsAsync(IEnumerable<Guid> statusIds)
    {
        return await _context.JAEventEntries
            .AsNoTracking()
            .Where(e => statusIds.Contains(e.Id))
            .Include(e => e.JAStatusEntry)
            .ToListAsync();
    }

    public async Task<JAEvent> AddAsync(JAEvent jaEvent)
    {
        _context.JAEventEntries.Add(jaEvent);
        await _context.SaveChangesAsync();
        
        return jaEvent;  
    }

    public async Task<JAEvent?> UpdateAsync(JAEvent updated)
    {
        var entity = await _context.JAEventEntries.FindAsync(updated.Id);
        if (entity is null) return null;
        
        entity.EventName = updated.EventName;
        entity.EventType = updated.EventType;
        entity.EventDate = updated.EventDate;
        entity.IsWholeDay = updated.IsWholeDay;
        entity.Note = updated.Note;
        
        await _context.SaveChangesAsync();
        
        return entity;  
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids)
    {
        var entities = await _context.JAEventEntries
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();

        _context.JAEventEntries.RemoveRange(entities);

        await _context.SaveChangesAsync();

        return true;
    }
}