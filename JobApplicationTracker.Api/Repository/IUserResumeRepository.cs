using JobApplicationTracker.DOs;

namespace JobApplicationTracker.Repository;

public interface IUserResumeRepository
{
    Task<UserResumeDo> CreateAsync(UserResumeDo resume);
    Task<UserResumeDo?> GetByIdAsync(Guid id);
    Task<UserResumeDo?> GetByUserAsync(Guid userId);
    Task<UserResumeDo?> UpdateAsync(UserResumeDo updated);
    Task<bool> DeleteAsync(Guid id);
}