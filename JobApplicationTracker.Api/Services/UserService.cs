using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<object> _passwordHasher = new();

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
            Username = u.Username,
            PasswordHash = u.PasswordHash,
            CreatedAt = u.CreatedAt,
            DeletedAt = u.DeletedAt
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
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            CreatedAt = user.CreatedAt,
            DeletedAt = user.DeletedAt
        };
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            CreatedAt = user.CreatedAt,
            DeletedAt = user.DeletedAt
        };
    }

    public async Task<UserDto> AddAsync(CreateUserDto user)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(user.Username.Trim());
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var hashedPassword = _passwordHasher.HashPassword(new object(), user.Password);

        var userDo = new UserDo
        {
            Username = user.Username.Trim(),
            PasswordHash = hashedPassword
        };

        var createdUser = await _userRepository.AddAsync(userDo);

        return new UserDto
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
            PasswordHash = createdUser.PasswordHash,
            CreatedAt = createdUser.CreatedAt,
            DeletedAt = createdUser.DeletedAt
        };
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto user)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);

        if (existingUser is null)
        {
            return null;
        }

        var userDo = new UserDo
        {
            Id = id,
            Username = user.Username is null ? existingUser.Username : user.Username.Trim(),
            PasswordHash = user.Password is null
                ? existingUser.PasswordHash
                : _passwordHasher.HashPassword(new object(), user.Password),
            CreatedAt = existingUser.CreatedAt,
            DeletedAt = existingUser.DeletedAt
        };

        var updatedUser = await _userRepository.UpdateAsync(userDo);

        if (updatedUser is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            PasswordHash = updatedUser.PasswordHash,
            CreatedAt = updatedUser.CreatedAt,
            DeletedAt = updatedUser.DeletedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}