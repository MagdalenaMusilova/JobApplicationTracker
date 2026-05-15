using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(string id);
}