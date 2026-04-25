using JobApplicationTracker.Dos;

namespace JobApplicationTracker.Repository;

public interface IUserRepository
{
    Task<IEnumerable<UserDo>> GetAllAsync();
    Task<UserDo?> GetByIdAsync(Guid id);
    Task<UserDo?> GetByUsernameAsync(string username);
    Task<UserDo> AddAsync(UserDo user);
    Task<UserDo?> UpdateAsync(UserDo user);
    Task<bool> DeleteAsync(Guid id);
}