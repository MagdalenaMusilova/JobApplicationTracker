using System.Security.Claims;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/mock-data")]
public class MockDataController : ControllerBase
{
    private readonly IMockDataService _mockDataService;

    public MockDataController(IMockDataService mockDataService)
    {
        _mockDataService = mockDataService;
    }

    [Authorize]
    [HttpPost("fill")]
    public async Task<IActionResult> FillMyAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponseDto("USER_ID_MISSING", "User ID was not found in the authentication token."));
        }

        await _mockDataService.FillAccountWithMockDataAsync(userId);
        return Ok(new { message = "Account populated with mock data successfully." });
    }
}
