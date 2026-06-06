using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthTokenService _tokenService;
    private readonly IAuthTokenRepository _tokenRepository;

    public AuthService(
        UserManager<User> userManager,
        IAuthTokenService tokenService,
        IAuthTokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _tokenRepository = tokenRepository;
    }

    public async Task<AuthServiceResultDto<SignInResponseDto>> SignInAsync(SignInDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid username or password.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
        {
            return AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid username or password.");
        }

        var response = await CreateTokenResponseAsync(user);

        return AuthServiceResultDto<SignInResponseDto>.Success(response);
    }

    public async Task<AuthServiceResultDto<SignInResponseDto>> SignUpAsync(SignUpDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser is not null)
        {
            return AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status400BadRequest,
                "Registration failed. Please use different email.");
        }

        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(error => error.Description));

            return AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status400BadRequest,
                $"Failed to create user: {errors}");
        }

        var response = await CreateTokenResponseAsync(user);

        return AuthServiceResultDto<SignInResponseDto>.Success(response);
    }

    public async Task<AuthServiceResultDto<SignInResponseDto>> RefreshAsync(RefreshTokenRequestDto dto)
    {
        var existingRefreshToken = await _tokenRepository.GetByTokenWithUserAsync(dto.RefreshToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive)
        {
            return AuthServiceResultDto<SignInResponseDto>.Failure(
                StatusCodes.Status401Unauthorized,
                "Invalid refresh token.");
        }

        existingRefreshToken.RevokedAt = DateTime.UtcNow;

        var response = await CreateTokenResponseAsync(existingRefreshToken.User);

        await _tokenRepository.SaveChangesAsync();

        return AuthServiceResultDto<SignInResponseDto>.Success(response);
    }

    public async Task LogoutAsync(LogoutRequestDto dto)
    {
        var refreshToken = await _tokenRepository.GetByTokenAsync(dto.RefreshToken);

        if (refreshToken is null)
        {
            return;
        }

        refreshToken.RevokedAt = DateTime.UtcNow;

        await _tokenRepository.SaveChangesAsync();
    }

    private async Task<SignInResponseDto> CreateTokenResponseAsync(User user)
    {
        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);

        return new SignInResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken.Token
        };
    }

    private async Task<RefreshToken> CreateRefreshTokenAsync(string userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _tokenRepository.AddAsync(refreshToken);
        await _tokenRepository.SaveChangesAsync();

        return refreshToken;
    }
}