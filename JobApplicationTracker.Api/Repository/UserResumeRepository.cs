using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models.UserProfile;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class UserResumeRepository : IUserResumeRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public UserResumeRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
        
    public async Task<UserResume> CreateAsync(UserResume resume)
    {
        _context.ResumeEntries.Add(resume);
        await _context.SaveChangesAsync();
        
        return resume;
    }

    public async Task<UserResume?> GetByIdAsync(Guid id)
    {
        return await _context.ResumeEntries
            .AsNoTracking()
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<UserResume?> GetByUserAsync(string userId)
    {
        return await _context.ResumeEntries
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<UserResume?> UpdateAsync(UserResume updated)
    {
        var entity = _context.ResumeEntries.Find(updated.Id);
        if (entity is null) return null;
        
        entity.UserId = entity.UserId;
        entity.WorkExperiences = _mapper.Map<List<WorkExperience>>(updated.WorkExperiences);
        entity.Education = _mapper.Map<List<Education>>(updated.Education);
        entity.Trainings = _mapper.Map<List<Training>>(updated.Trainings);
        entity.Skills = _mapper.Map<List<ResumeSkill>>(updated.Skills);
        entity.UncategorizedExperiences = updated.UncategorizedExperiences;

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.ResumeEntries.FindAsync(id);

        if (entity == null)
            return false;

        _context.ResumeEntries.Remove(entity);

        await _context.SaveChangesAsync();

        return true;
    }
}