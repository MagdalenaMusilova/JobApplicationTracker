using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Test.Controllers;

public class ProfileControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly ProfileController _controller;

    public ProfileControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new ProfileController(_mockUserService.Object);
    }

    private void SetupUser(string userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Fact]
    public async Task GetMe_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var userId = "user123";
        var userAccountDto = new UserAccountDto { Id = userId, Username = "testuser", Email = "test@test.com" };
        
        SetupUser(userId);
        _mockUserService.Setup(s => s.GetAccountByIdAsync(userId)).ReturnsAsync(userAccountDto);

        // Act
        var result = await _controller.GetMe();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedUser = okResult.Value.Should().BeOfType<UserAccountDto>().Subject;
        returnedUser.Id.Should().Be(userId);
        returnedUser.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task GetMe_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        
        SetupUser(userId);
        _mockUserService.Setup(s => s.GetAccountByIdAsync(userId)).ReturnsAsync((UserAccountDto?)null);

        // Act
        var result = await _controller.GetMe();

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }
}