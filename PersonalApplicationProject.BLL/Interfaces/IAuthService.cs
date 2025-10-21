using PersonalApplicationProject.BLL.DTOs;
using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.BLL.Interfaces;

public interface IAuthService
{
    Task<Result<UserDto>> RegisterAsync(RegisterRequestDto request);

    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}