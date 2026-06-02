using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IAuthService
{
    Task<AuthServiceResultDto<SignInResponseDto>> SignInAsync(SignInDto dto);

    Task<AuthServiceResultDto<SignInResponseDto>> SignUpAsync(SignUpDto dto);

    Task<AuthServiceResultDto<SignInResponseDto>> RefreshAsync(RefreshTokenRequestDto dto);

    Task LogoutAsync(LogoutRequestDto dto);
}