using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto user)
    {
        var createdUser = await _userService.AddAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto user)
    {
        var updatedUser = await _userService.UpdateAsync(id, user);

        if (updatedUser is null)
        {
            return NotFound();
        }

        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}