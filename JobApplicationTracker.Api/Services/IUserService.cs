using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<UserDto> AddAsync(CreateUserDto user);
    Task<UserDto?> UpdateAsync(int id, UpdateUserDto user);
    Task<bool> DeleteAsync(int id);
}