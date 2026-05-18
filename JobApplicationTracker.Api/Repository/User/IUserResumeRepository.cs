using JobApplicationTracker.Models.UserProfile;

namespace JobApplicationTracker.Repository;

public interface IUserResumeRepository
{
    Task<UserResume> CreateAsync(UserResume resume);
    Task<UserResume?> GetByIdAsync(Guid id);
    Task<UserResume?> GetByUserAsync(string userId);
    Task<UserResume?> UpdateAsync(UserResume updated);
    Task<bool> DeleteAsync(Guid id);
}