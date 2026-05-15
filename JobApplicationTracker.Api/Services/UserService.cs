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

    public async Task<UserDto?> GetByIdAsync(string id)
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

    public async Task<bool> DeleteAsync(string id)
    {
        return await _userRepository.DeleteAsync(id);
    }
}