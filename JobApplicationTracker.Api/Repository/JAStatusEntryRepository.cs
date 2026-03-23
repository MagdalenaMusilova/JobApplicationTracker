using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JAStatusEntryRepository : IJAStatusEntryRepository
{
    private readonly JAStatusEntryDbContext _context;
    private readonly IMapper _mapper;
    
    public JAStatusEntryRepository(JAStatusEntryDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<JAStatusEntryDo> AddAsync(JAStatusEntryDo jaStatusEntry)
    {
        var entity = _mapper.Map<JAStatusEntry>(jaStatusEntry);
        _context.JAStatusEntries.Add(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<JAStatusEntryDo>(entity);
    }

    public async Task<bool> DeleteBulkAsync(IEnumerable<int> ids)
    {
        await _context.JAStatusEntries
            .Where(e => ids.Contains(e.Id))
            .ExecuteDeleteAsync();
        return true;
    }
}