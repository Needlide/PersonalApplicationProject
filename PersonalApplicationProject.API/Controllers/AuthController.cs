using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.BLL.DTOs.User;
using PersonalApplicationProject.BLL.Interfaces;

namespace PersonalApplicationProject.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var result = await authService.RegisterAsync(registerRequestDto);

        return !result.IsSuccess ? Conflict(result.Error) : Ok(result.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var result = await authService.LoginAsync(loginRequestDto);
        
        return !result.IsSuccess ? Unauthorized(result.Error):Ok(result.Value);
    }
}