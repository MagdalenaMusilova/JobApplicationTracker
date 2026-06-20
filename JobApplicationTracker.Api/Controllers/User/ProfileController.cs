using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserAccountDto>> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        var account = await _userService.GetAccountByIdAsync(userId);
        return account is null
            ? NotFound(new ErrorResponseDto("USER_NOT_FOUND", "User account was not found."))
            : Ok(account);
    }

    [HttpPut("email")]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        var result = await _userService.UpdateEmailAsync(userId, dto.Email);
        if (result.Succeeded)
        {
            return Ok();
        }
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest(new ErrorResponseDto("UPDATE_EMAIL_FAILED", errors));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        var result = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

        if (result.Succeeded)
        {
            return Ok();
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest(new ErrorResponseDto("CHANGE_PASSWORD_FAILED", errors));
    }
}