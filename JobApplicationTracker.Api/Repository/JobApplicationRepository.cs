using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly JobApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public JobApplicationRepository(JobApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<JobApplicationDo>> GetAllAsync()
    {
        return await _context.JobApplications
            .AsNoTracking()
            .Select(ja => _mapper.Map<JobApplicationDo>(ja))
            .ToListAsync();
    }

    public async Task<IEnumerable<JobApplicationDo>> GetAllByUserAsync(int userId)
    {
        return await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.UserId == userId)
            .Select(ja => _mapper.Map<JobApplicationDo>(ja))
            .ToListAsync();
    }

    public async Task<JobApplicationDo?> GetByIdAsync(int id)
    {
        return await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.Id == id)
            .Select(ja => _mapper.Map<JobApplicationDo>(ja))
            .FirstOrDefaultAsync();    
    }

    public async Task<JobApplicationDo> AddAsync(JobApplicationDo application)
    {
        var entity = _mapper.Map<JobApplication>(application);
        _context.JobApplications.Add(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<JobApplicationDo>(entity);   
    }

    public async Task<JobApplicationDo> UpdateAsync(JobApplicationDo application)
    {
        var existingApplication = await _context.JobApplications.FindAsync(application.Id);

        if (existingApplication is null)
        {
            return null;
        }

        existingApplication.Id = application.Id;
        existingApplication.UserId = application.UserId;
        existingApplication.Company = application.Company;
        existingApplication.Position = application.Position;
        existingApplication.StatusHistory = application.StatusHistory.Select(e => _mapper.Map<JAStatusEntry>(e));
        existingApplication.Note = application.Note;

        await _context.SaveChangesAsync();

        return _mapper.Map<JobApplicationDo>(existingApplication);    
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var application = await _context.JobApplications.FindAsync(id);

        if (application is null)
        {
            return false;
        }
        
        _context.JobApplications.Remove(application);
        await _context.SaveChangesAsync();
        return true;   
    }
}