using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username
        });
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public async Task<UserDto> AddAsync(CreateUserDto user)
    {
        var userDo = new UserDo
        {
            Username = user.Username.Trim()
        };

        var createdUser = await _userRepository.AddAsync(userDo);

        return new UserDto
        {
            Id = createdUser.Id,
            Username = createdUser.Username
        };
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto user)
    {
        var userDo = new UserDo
        {
            Id = id,
            Username = user.Username.Trim()
        };

        var updatedUser = await _userRepository.UpdateAsync(userDo);

        if (updatedUser is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}