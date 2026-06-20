using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<SignInResponseDto>> SignIn([FromBody] SignInDto dto)
    {
        var result = await _authService.SignInAsync(dto);

        return ToActionResult(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<SignInResponseDto>> SignUp([FromBody] SignUpDto dto)
    {
        var result = await _authService.SignUpAsync(dto);

        return ToActionResult(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<SignInResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshAsync(dto);

        return ToActionResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto dto)
    {
        try
        {
            await _authService.LogoutAsync(dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDto("LOGOUT_FAILED", ex.Message));
        }
    }

    private ActionResult<SignInResponseDto> ToActionResult(AuthServiceResultDto<SignInResponseDto> result)
    {
        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        var errorResponse = new ErrorResponseDto(
            result.StatusCode.ToString(),
            result.ErrorMessage
        );
        return StatusCode(result.StatusCode, errorResponse);
    }
}