using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<bool> DeleteAsync(string id);
}