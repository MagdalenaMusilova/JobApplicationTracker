using AutoMapper;
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
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => _mapper.Map<UserDto>(u));
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user is null)
        {
            return null;
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> AddAsync(CreateUserDto user)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(user.Username.Trim());
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var hashedPassword = _passwordHasher.HashPassword(new object(), user.Password);

        var userDo = new UserDo     // not mapped because adding fields
        {
            Username = user.Username.Trim(),
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
            DeletedAt = null
        };

        var createdUser = await _userRepository.AddAsync(userDo);

        return _mapper.Map<UserDto>(createdUser);
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto user)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);

        if (existingUser is null)
        {
            return null;
        }

        var userDo = new UserDo // not mapped because adjusting fields
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

        return _mapper.Map<UserDto>(updatedUser);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}