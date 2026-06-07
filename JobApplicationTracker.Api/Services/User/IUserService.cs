using JobApplicationTracker.DTOs;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserAccountDto?> GetAccountByIdAsync(string id);
    Task<bool> UpdateEmailAsync(string userId, string newEmail);
    Task<bool> UpdateUsernameAsync(string userId, string newUsername);
    Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> DeleteAsync(string id);
}