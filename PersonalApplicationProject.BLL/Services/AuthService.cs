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
        var refreshToken = GenerateJwt.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await unitOfWork.SaveChangesAsync();

        var response = new LoginResponseDto
        {
            Token = token,
            Expiration = expiration,
            RefreshToken = refreshToken,
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

    public async Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var user = (await unitOfWork.Users.FindAsync(u => u.RefreshToken == request.RefreshToken)).FirstOrDefault();

        if (user is null) return Result<LoginResponseDto>.Failure("Invalid refresh token.");

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            user.RefreshToken = null;
            await unitOfWork.SaveChangesAsync();
            return Result<LoginResponseDto>.Failure("Refresh token has expired. Please log in again.");
        }

        var (newJwtToken, newJwtExpiration) = GenerateJwt.GenerateJwtToken(user, jwtOptions);

        var newRefreshToken = GenerateJwt.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await unitOfWork.SaveChangesAsync();

        var response = new LoginResponseDto
        {
            Token = newJwtToken,
            Expiration = newJwtExpiration,
            RefreshToken = newRefreshToken,
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

    public async Task<Result<bool>> LogoutAsync(int userId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId);

        if (user is null) return Result<bool>.Success(true);

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}