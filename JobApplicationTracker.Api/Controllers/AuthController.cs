using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobApplicationTracker.Database;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<object> _passwordHasher = new();

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("signin")]
    public async Task<ActionResult<SignInResponseDto>> SignIn([FromBody] SignInDto dto)
    {
        var user = await _userService.GetByUsernameAsync(dto.Username);

        if (user is null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            new object(),
            user.PasswordHash,
            dto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid username or password.");
        }

        var jwt = _configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return Ok(new SignInResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}