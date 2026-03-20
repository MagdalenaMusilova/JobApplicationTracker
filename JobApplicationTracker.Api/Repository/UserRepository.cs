using JobApplicationTracker.Database;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDo>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Select(u => new UserDo
            {
                Id = u.Id,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                CreatedAt = u.CreatedAt,
                DeletedAt = u.DeletedAt
            })
            .ToListAsync();
    }

    public async Task<UserDo?> GetByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDo
            {
                Id = u.Id,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                CreatedAt = u.CreatedAt,
                DeletedAt = u.DeletedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserDo?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .Select(u => new UserDo
            {
                Id = u.Id,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                CreatedAt = u.CreatedAt,
                DeletedAt = u.DeletedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserDo> AddAsync(UserDo user)
    {
        var entity = new User
        {
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            CreatedAt = user.CreatedAt,
            DeletedAt = user.DeletedAt
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        return new UserDo
        {
            Id = entity.Id,
            Username = entity.Username,
            PasswordHash = entity.PasswordHash,
            CreatedAt = entity.CreatedAt,
            DeletedAt = entity.DeletedAt
        };
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

        return new UserDo
        {
            Id = existingUser.Id,
            Username = existingUser.Username,
            PasswordHash = existingUser.PasswordHash,
            CreatedAt = existingUser.CreatedAt,
            DeletedAt = existingUser.DeletedAt
        };
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