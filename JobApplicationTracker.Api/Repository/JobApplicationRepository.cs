using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _context;
    
    public JobApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<JobApplication> Query(Guid userId)
    {
        return _context.JobApplications.AsQueryable()
            .Where(ja => ja.UserId == userId);
    }

    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Include(ja => ja.StatusHistory)
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JobApplication>> GetAllByUserAsync(Guid userId)
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.UserId == userId)
            .Include(ja => ja.StatusHistory)
            .ToListAsync();
        return res;
    }

    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.Id == id)
            .Include(ja => ja.StatusHistory)
            .FirstOrDefaultAsync();    
        return res;
    }

    public async Task<JobApplication> AddAsync(JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();
        
        return application;   
    }

    public async Task<JobApplication> UpdateAsync(JobApplication application)
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
        existingApplication.StatusHistory = application.StatusHistory;
        existingApplication.Note = application.Note;

        await _context.SaveChangesAsync();

        return existingApplication;    
    }

    public async Task<bool> DeleteAsync(Guid id)
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