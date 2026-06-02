using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task SignIn_WithValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Email = "test@example.com",
            Password = "Test123!"
        };

        var expectedResponse = new SignInResponseDto
        {
            Token = "jwt-token-here",
            RefreshToken = "refresh-token-here"
        };

        _mockAuthService
            .Setup(service => service.SignInAsync(signInDto))
            .ReturnsAsync(AuthServiceResultDto<SignInResponseDto>.Success(expectedResponse));

        // Act
        var result = await _controller.SignIn(signInDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<SignInResponseDto>().Subject;

        response.Token.Should().Be(expectedResponse.Token);
        response.RefreshToken.Should().Be(expectedResponse.RefreshToken);

        _mockAuthService.Verify(service => service.SignInAsync(signInDto), Times.Once);
    }

    [Fact]
    public async Task SignIn_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Email = "test@example.com",
            Password = "WrongPassword!"
        };

        _mockAuthService
            .Setup(service => service.SignInAsync(signInDto))
            .ReturnsAsync(AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid username or password."));

        // Act
        var result = await _controller.SignIn(signInDto);

        // Assert
        var unauthorizedResult = result.Result.Should().BeOfType<ObjectResult>().Subject;

        unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        unauthorizedResult.Value.Should().Be("Invalid username or password.");

        _mockAuthService.Verify(service => service.SignInAsync(signInDto), Times.Once);
    }

    [Fact]
    public async Task SignUp_WithValidData_ReturnsOkWithToken()
    {
        // Arrange
        var signUpDto = new SignUpDto
        {
            Email = "newuser@example.com",
            Password = "NewPass123!"
        };

        var expectedResponse = new SignInResponseDto
        {
            Token = "new-jwt-token",
            RefreshToken = "new-refresh-token"
        };

        _mockAuthService
            .Setup(service => service.SignUpAsync(signUpDto))
            .ReturnsAsync(AuthServiceResultDto<SignInResponseDto>.Success(expectedResponse));

        // Act
        var result = await _controller.SignUp(signUpDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<SignInResponseDto>().Subject;

        response.Token.Should().Be(expectedResponse.Token);
        response.RefreshToken.Should().Be(expectedResponse.RefreshToken);

        _mockAuthService.Verify(service => service.SignUpAsync(signUpDto), Times.Once);
    }

    [Fact]
    public async Task SignUp_WithExistingEmail_ReturnsBadRequest()
    {
        // Arrange
        var signUpDto = new SignUpDto
        {
            Email = "existing@example.com",
            Password = "Pass123!"
        };

        _mockAuthService
            .Setup(service => service.SignUpAsync(signUpDto))
            .ReturnsAsync(AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status400BadRequest,
                "Email is already being used."));

        // Act
        var result = await _controller.SignUp(signUpDto);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<ObjectResult>().Subject;

        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be("Email is already being used.");

        _mockAuthService.Verify(service => service.SignUpAsync(signUpDto), Times.Once);
    }

    [Fact]
    public async Task Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens()
    {
        // Arrange
        var refreshDto = new RefreshTokenRequestDto
        {
            RefreshToken = "existing-refresh-token"
        };

        var expectedResponse = new SignInResponseDto
        {
            Token = "new-access-token",
            RefreshToken = "new-refresh-token"
        };

        _mockAuthService
            .Setup(service => service.RefreshAsync(refreshDto))
            .ReturnsAsync(AuthServiceResultDto<SignInResponseDto>.Success(expectedResponse));

        // Act
        var result = await _controller.Refresh(refreshDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<SignInResponseDto>().Subject;

        response.Token.Should().Be(expectedResponse.Token);
        response.RefreshToken.Should().Be(expectedResponse.RefreshToken);

        _mockAuthService.Verify(service => service.RefreshAsync(refreshDto), Times.Once);
    }

    [Fact]
    public async Task Refresh_WithInvalidRefreshToken_ReturnsUnauthorized()
    {
        // Arrange
        var refreshDto = new RefreshTokenRequestDto
        {
            RefreshToken = "invalid-refresh-token"
        };

        _mockAuthService
            .Setup(service => service.RefreshAsync(refreshDto))
            .ReturnsAsync(AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid refresh token."));

        // Act
        var result = await _controller.Refresh(refreshDto);

        // Assert
        var unauthorizedResult = result.Result.Should().BeOfType<ObjectResult>().Subject;

        unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        unauthorizedResult.Value.Should().Be("Invalid refresh token.");

        _mockAuthService.Verify(service => service.RefreshAsync(refreshDto), Times.Once);
    }

    [Fact]
    public async Task Logout_ReturnsNoContent()
    {
        // Arrange
        var logoutDto = new LogoutRequestDto
        {
            RefreshToken = "refresh-token-to-logout"
        };

        _mockAuthService
            .Setup(service => service.LogoutAsync(logoutDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Logout(logoutDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mockAuthService.Verify(service => service.LogoutAsync(logoutDto), Times.Once);
    }
}