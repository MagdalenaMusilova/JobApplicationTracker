using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public interface IAuthTokenService
{
    public string GenerateToken(User user);
}