using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JAStatusEntryRepository : IJAStatusEntryRepository
{
    private readonly AppDbContext _context;
    
    public JAStatusEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<JAStatusEntry?> GetByIdAsync(Guid id)
    {
        return await _context.JAStatusEntries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();   
    }

    public async Task<IEnumerable<JAStatusEntry>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationIds)
    {
        return await _context.JAStatusEntries
            .AsNoTracking()
            .Where(e => jobApplicationIds.Contains(e.JobApplicationId))
            .ToListAsync();
    }

    public async Task<JAStatusEntry?> AddAsync(JAStatusEntry jaStatusEntry)
    {
        _context.JAStatusEntries.Add(jaStatusEntry);
        await _context.SaveChangesAsync();
        
        return jaStatusEntry;
    }

    public async Task<JAStatusEntry?> UpdateAsync(Guid id, JAStatusEntry updated)
    {
        var entity = await _context.JAStatusEntries.FindAsync(id);
        if (entity is null) return null;

        entity.JaStatusType = updated.JaStatusType;
        entity.Note = updated.Note;

        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids)
    {
        await _context.JAStatusEntries
            .Where(e => ids.Contains(e.Id))
            .ExecuteDeleteAsync();
        return true;
    }
}