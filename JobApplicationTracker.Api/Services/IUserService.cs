using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<bool> DeleteAsync(string id);
}