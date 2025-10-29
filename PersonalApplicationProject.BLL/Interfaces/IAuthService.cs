using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.BLL.Interfaces;

public interface IAuthService
{
    Task<Result<UserDto>> RegisterAsync(RegisterRequestDto request);

    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);

    Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);

    Task<Result<bool>> LogoutAsync(int userId);
}