using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto user)
    {
        var createdUser = await _userService.AddAsync(user);
        return Ok(createdUser);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await _userService.GetByIdAsync(userId);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateMe([FromBody] UpdateUserDto user)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var updated = await _userService.UpdateAsync(userId, user);
        return updated is null ? NotFound() : Ok(updated);
    }
}