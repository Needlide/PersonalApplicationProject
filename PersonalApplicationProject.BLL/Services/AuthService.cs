using PersonalApplicationProject.BLL.DTOs.User;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.BLL.Options;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.BLL.Services;

public class AuthService(IUnitOfWork unitOfWork, JwtOptions jwtOptions) : IAuthService
{
    public async Task<Result<UserDto>> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await unitOfWork.Users.GetByEmailAsync(request.Email);

        if (existingUser is not null) return Result<UserDto>.Failure("User already exists");

        var newUser = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = PasswordHasher.HashPassword(request.Password)
        };

        await unitOfWork.Users.AddAsync(newUser);
        await unitOfWork.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = newUser.Id,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email
        };

        return Result<UserDto>.Success(userDto);
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(
        LoginRequestDto request)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(request.Email);

        if (user is null) return Result<LoginResponseDto>.Failure("Invalid credentials");

        var isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (!isPasswordValid) return Result<LoginResponseDto>.Failure("Invalid credentials");

        var (token, expiration) = GenerateJwt.GenerateJwtToken(user, jwtOptions);

        var response = new LoginResponseDto
        {
            Token = token,
            Expiration = expiration,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };

        return Result<LoginResponseDto>.Success(response);
    }
}