using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobApplicationTracker.Database;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthTokenService _tokenService;

    public AuthController(UserManager<User> userManager, IAuthTokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("signin")]
    public async Task<ActionResult<SignInResponseDto>> SignIn([FromBody] SignInDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new SignInResponseDto
        {
            Token = token
        });
    }

    [HttpPost("signup")]
    public async Task<ActionResult<SignInResponseDto>> SignUp([FromBody] SignUpDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser is not null)
        {
            return BadRequest("Email is already being used.");
        }

        var user = new User
        {
            Email = dto.Email,
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return BadRequest($"Failed to create user: {errors}");
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new SignInResponseDto
        {
            Token = token
        });
    }
}