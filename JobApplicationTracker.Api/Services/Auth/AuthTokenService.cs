using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobApplicationTracker.Models;
using Microsoft.IdentityModel.Tokens;

namespace JobApplicationTracker.Services;

public class AuthTokenService : IAuthTokenService
{
    private readonly IConfiguration _configuration;

    public AuthTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var jwt = _configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id)
        };
    
        // Only add claims if they have non-null and non-whitespace values
        if (!string.IsNullOrWhiteSpace(user.UserName))
        {
            claims.Add(new Claim("name", user.UserName));
        }
    
        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim("email", user.Email));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            notBefore: now,
            expires: now.AddHours(2),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
        // Debug: verify claims are in the token before writing
        System.Diagnostics.Debug.WriteLine($"Claims count: {claims.Count}");
        foreach (var claim in claims)
        {
            System.Diagnostics.Debug.WriteLine($"  Claim: {claim.Type} = {claim.Value}");
        }
            
        return tokenString;
    }
    
}