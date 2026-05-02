using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.DOs;
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
        
    public async Task<UserResumeDo> CreateAsync(UserResumeDo resume)
    {
        var entity = _mapper.Map<UserResume>(resume);
        _context.ResumeEntries.Add(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<UserResumeDo>(entity);
    }

    public async Task<UserResumeDo?> GetByIdAsync(Guid id)
    {
        return await _context.ResumeEntries
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => _mapper.Map<UserResumeDo>(r))
            .FirstOrDefaultAsync();
    }

    public async Task<UserResumeDo?> GetByUserAsync(Guid userId)
    {
        return await _context.ResumeEntries
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Select(r => _mapper.Map<UserResumeDo>(r))
            .FirstOrDefaultAsync();
    }

    public async Task<UserResumeDo?> UpdateAsync(UserResumeDo updated)
    {
        var entity = _context.ResumeEntries.Find(updated.Id);
        if (entity is null) return null;
        
        entity.UserId = entity.UserId;
        entity.WorkExperiences = _mapper.Map<List<WorkExperience>>(updated.WorkExperiences);
        entity.Education = _mapper.Map<List<Education>>(updated.Education);
        entity.Trainings = _mapper.Map<List<Training>>(updated.Trainings);
        entity.Skills = _mapper.Map<List<ResumeSkill>>(updated.Skills);
        entity.UncategorizedExperiences = _mapper.Map<List<OtherExperience>>(updated.UncategorizedSkillUsages);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserResumeDo>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _context.ResumeEntries
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();
        return true;   
    }
}