using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JAStatusEntryRepository : IJAStatusEntryRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public JAStatusEntryRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JAStatusEntryDo?> GetByIdAsync(Guid id)
    {
        return await _context.JAStatusEntries
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => _mapper.Map<JAStatusEntryDo>(e))
            .FirstOrDefaultAsync();   
    }

    public async Task<IEnumerable<JAStatusEntryDo>> GetByJobApplicationIdsAsync(IEnumerable<Guid> jobApplicationIds)
    {
        return await _context.JAStatusEntries
            .AsNoTracking()
            .Where(e => jobApplicationIds.Contains(e.JobApplicationId))
            .Select(e => _mapper.Map<JAStatusEntryDo>(e))
            .ToListAsync();
    }

    public async Task<JAStatusEntryDo> AddAsync(JAStatusEntryDo jaStatusEntry)
    {
        var entity = _mapper.Map<JAStatusEntry>(jaStatusEntry);
        _context.JAStatusEntries.Add(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<JAStatusEntryDo>(entity);
    }

    public async Task<JAStatusEntryDo?> UpdateAsync(Guid id, JAStatusEntryDo updated)
    {
        var entity = await _context.JAStatusEntries.FindAsync(id);
        if (entity is null) return null;

        entity.JaStatusType = updated.JaStatusType;
        entity.Note = updated.Note;

        await _context.SaveChangesAsync();
        return _mapper.Map<JAStatusEntryDo>(entity);
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<Guid> ids)
    {
        await _context.JAStatusEntries
            .Where(e => ids.Contains(e.Id))
            .ExecuteDeleteAsync();
        return true;
    }
}