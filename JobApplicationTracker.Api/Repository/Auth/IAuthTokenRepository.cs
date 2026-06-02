using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repository;

public interface IAuthTokenRepository
{
    Task<RefreshToken?> GetByTokenWithUserAsync(string token);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task AddAsync(RefreshToken refreshToken);

    Task SaveChangesAsync();
}