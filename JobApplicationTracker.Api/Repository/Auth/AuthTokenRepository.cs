using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repository;

public class AuthTokenRepository : IAuthTokenRepository
{
    private readonly AppDbContext _dbContext;

    public AuthTokenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken?> GetByTokenWithUserAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}