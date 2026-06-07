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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var account = await _userService.GetAccountByIdAsync(userId);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpPut("email")]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var success = await _userService.UpdateEmailAsync(userId, dto.Email);
        return success ? Ok() : BadRequest(new ErrorResponseDto("UPDATE_EMAIL_FAILED", "Failed to update email."));
    }

    [HttpPut("username")]
    public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var success = await _userService.UpdateUsernameAsync(userId, dto.Username);
        return success ? Ok() : BadRequest(new ErrorResponseDto("UPDATE_USERNAME_FAILED", "Failed to update username."));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

        if (result.Succeeded)
        {
            return Ok();
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest(new ErrorResponseDto("CHANGE_PASSWORD_FAILED", errors));
    }
}