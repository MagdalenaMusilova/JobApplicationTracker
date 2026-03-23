using AutoMapper;
using JobApplicationTracker.Database;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDo>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Select(u => _mapper.Map<UserDo>(u))
            .ToListAsync();
    }

    public async Task<UserDo?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => _mapper.Map<UserDo>(u))
            .FirstOrDefaultAsync();
    }

    public async Task<UserDo?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .Select(u => _mapper.Map<UserDo>(u))
            .FirstOrDefaultAsync();
    }

    public async Task<UserDo> AddAsync(UserDo user)
    {
        var entity = _mapper.Map<User>(user);

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDo>(entity);
    }

    public async Task<UserDo?> UpdateAsync(UserDo user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser is null)
        {
            return null;
        }

        existingUser.Username = user.Username;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.CreatedAt = user.CreatedAt;
        existingUser.DeletedAt = user.DeletedAt;

        await _context.SaveChangesAsync();

        return _mapper.Map<UserDo>(existingUser);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}