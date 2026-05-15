using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto> AddAsync(CreateUserDto user);
    Task<UserDto?> UpdateAsync(string id, UpdateUserDto user);
    Task<bool> DeleteAsync(string id);
}