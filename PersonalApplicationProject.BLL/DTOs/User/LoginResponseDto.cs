namespace PersonalApplicationProject.BLL.DTOs.User;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public UserDto User { get; set; } = null!;
}