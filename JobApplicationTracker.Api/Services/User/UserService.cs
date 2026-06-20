using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, UserManager<User> userManager, IMapper mapper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => _mapper.Map<UserDto>(u));
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserAccountDto?> GetAccountByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return new UserAccountDto
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.UserName!,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<IdentityResult> UpdateEmailAsync(string userId, string newEmail)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var userByEmail = await _userManager.FindByEmailAsync(newEmail);
        if (userByEmail != null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Failed to update email address. Please use a different email address." });
        }

        user.Email = newEmail;
        user.UserName = newEmail;

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}