using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Enums;
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

    public async Task<IEnumerable<JobApplication>> GetAllAsync()
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Include(ja => ja.StatusHistory
                .OrderBy(stat => stat.OrderIndex))
            .ThenInclude(ja => ja.JAEvent)
            .Include(ja => ja.JobListing)
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JobApplicationMinimal>> GetAllNotFinishedAsync(string userId)
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.UserId == userId)
            .Include(ja => ja.StatusHistory.OrderBy(stat => stat.OrderIndex))
            .ToListAsync();
        
        var notFinished = res
            .Where(ja => ja.StatusHistory.Any() && 
                         (int)ja.StatusHistory.OrderByDescending(s => s.OrderIndex).First().JaStatusType < (int)JAStatusType.Accepted)
            .Select(ja => new JobApplicationMinimal
            {
                JAId = ja.Id,
                Company = ja.Company,
                Position = ja.Position,
                UserId = ja.UserId,
                JAStatus = ja.StatusHistory.OrderByDescending(s => s.OrderIndex).First().JaStatusType
            })
            .ToList();
        
        return notFinished;
    }

    public async Task<IEnumerable<JobApplication>> GetAllByUserAsync(string userId)
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.UserId == userId)
            .Include(ja => ja.StatusHistory
                .OrderBy(stat => stat.OrderIndex))
            .ThenInclude(ja => ja.JAEvent)
            .Include(ja => ja.JobListing)
            .ToListAsync();
        return res;
    }

    public async Task<IEnumerable<JobApplicationMinimal>> GetAllByUserMinimalAsync(string userId, bool? archived)
    {
        var queryable = _context.JaMinimalView
            .AsNoTracking()
            .Where(ja => ja.UserId == userId);

        if (archived.HasValue)
        {
            if (archived.Value)
            {
                // Archived: status >= Accepted (1000)
                queryable = queryable.Where(ja => ja.JAStatus >= JAStatusType.Accepted);
            }
            else
            {
                // Non-archived: status < Accepted (1000)
                queryable = queryable.Where(ja => ja.JAStatus < JAStatusType.Accepted);
            }
        }
        // If archived is null, don't filter (fetch all)

        var res = await queryable.ToListAsync();
        return res;
    }

    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        var res = await _context.JobApplications
            .AsNoTracking()
            .Where(ja => ja.Id == id)
            .Include(ja => ja.StatusHistory
                .OrderBy(stat => stat.OrderIndex))
            .ThenInclude(ja => ja.JAEvent)
            .Include(ja => ja.JobListing)
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
        
        existingApplication.Company = application.Company;
        existingApplication.Position = application.Position;
        existingApplication.StatusHistory = application.StatusHistory.OrderBy(stat => stat.OrderIndex).ToList();
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