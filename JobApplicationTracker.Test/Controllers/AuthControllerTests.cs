using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test.Controllers;

public class AuthControllerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IAuthTokenService> _mockTokenService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);
        
        _mockTokenService = new Mock<IAuthTokenService>();
        _controller = new AuthController(_mockUserManager.Object, _mockTokenService.Object);
    }
    
    [Fact]
    public async Task SignIn_WithValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Username = "testuser",
            Password = "Test123!"
        };

        var user = new User
        {
            Id = "user-id-123",
            UserName = "testuser",
            Email = "test@example.com"
        };

        var expectedToken = "jwt-token-here";

        _mockUserManager.Setup(x => x.FindByNameAsync(signInDto.Username))
            .ReturnsAsync(user);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, signInDto.Password))
            .ReturnsAsync(true);
        _mockTokenService.Setup(x => x.GenerateToken(user))
            .Returns(expectedToken);

        // Act
        var result = await _controller.SignIn(signInDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var response = okResult!.Value as SignInResponseDto;
        response.Should().NotBeNull();
        response!.Token.Should().Be(expectedToken);

        _mockUserManager.Verify(x => x.FindByNameAsync(signInDto.Username), Times.Once);
        _mockUserManager.Verify(x => x.CheckPasswordAsync(user, signInDto.Password), Times.Once);
        _mockTokenService.Verify(x => x.GenerateToken(user), Times.Once);
    }

    [Fact]
    public async Task SignIn_WithInvalidUsername_ReturnsUnauthorized()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Username = "nonexistent",
            Password = "Test123!"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(signInDto.Username))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.SignIn(signInDto);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid username or password.");

        _mockUserManager.Verify(x => x.FindByNameAsync(signInDto.Username), Times.Once);
        _mockUserManager.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignIn_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var signInDto = new SignInDto
        {
            Username = "testuser",
            Password = "WrongPassword!"
        };

        var user = new User
        {
            Id = "user-id-123",
            UserName = "testuser",
            Email = "test@example.com"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(signInDto.Username))
            .ReturnsAsync(user);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, signInDto.Password))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.SignIn(signInDto);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid username or password.");

        _mockUserManager.Verify(x => x.FindByNameAsync(signInDto.Username), Times.Once);
        _mockUserManager.Verify(x => x.CheckPasswordAsync(user, signInDto.Password), Times.Once);
        _mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignUp_WithValidData_ReturnsOkWithToken()
    {
        // Arrange
        var signUpDto = new SignUpDto
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "NewPass123!"
        };

        var expectedToken = "new-jwt-token";

        _mockUserManager.Setup(x => x.FindByNameAsync(signUpDto.Username))
            .ReturnsAsync((User?)null);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), signUpDto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _mockTokenService.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(expectedToken);

        // Act
        var result = await _controller.SignUp(signUpDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var response = okResult!.Value as SignInResponseDto;
        response.Should().NotBeNull();
        response!.Token.Should().Be(expectedToken);

        _mockUserManager.Verify(x => x.FindByNameAsync(signUpDto.Username), Times.Once);
        _mockUserManager.Verify(x => x.CreateAsync(
            It.Is<User>(u => u.UserName == signUpDto.Username && u.Email == signUpDto.Email),
            signUpDto.Password), Times.Once);
        _mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignUp_WithExistingUsername_ReturnsBadRequest()
    {
        // Arrange
        var signUpDto = new SignUpDto
        {
            Username = "existinguser",
            Email = "new@example.com",
            Password = "Pass123!"
        };

        var existingUser = new User
        {
            Id = "existing-id",
            UserName = "existinguser",
            Email = "existing@example.com"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(signUpDto.Username))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _controller.SignUp(signUpDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Username already exists.");

        _mockUserManager.Verify(x => x.FindByNameAsync(signUpDto.Username), Times.Once);
        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignUp_WithInvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var signUpDto = new SignUpDto
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "weak"
        };

        var identityErrors = new[]
        {
            new IdentityError { Code = "PasswordTooShort", Description = "Password must be at least 6 characters." },
            new IdentityError { Code = "PasswordRequiresDigit", Description = "Password must have at least one digit." }
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(signUpDto.Username))
            .ReturnsAsync((User?)null);
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), signUpDto.Password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        // Act
        var result = await _controller.SignUp(signUpDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        var errorMessage = badRequestResult!.Value as string;
        errorMessage.Should().Contain("Failed to create user");
        errorMessage.Should().Contain("Password must be at least 6 characters");
        errorMessage.Should().Contain("Password must have at least one digit");

        _mockUserManager.Verify(x => x.FindByNameAsync(signUpDto.Username), Times.Once);
        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), signUpDto.Password), Times.Once);
        _mockTokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}