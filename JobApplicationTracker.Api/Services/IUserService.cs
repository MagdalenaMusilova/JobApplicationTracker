using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto> AddAsync(CreateUserDto user);
    Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto user);
    Task<bool> DeleteAsync(Guid id);
}