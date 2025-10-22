using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.BLL.DTOs.User;
using PersonalApplicationProject.BLL.Interfaces;

namespace PersonalApplicationProject.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, IValidator<RegisterRequestDto> registerRequestDtoValidator, IValidator<LoginRequestDto> loginRequestDtoValidator) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var validationResult = await registerRequestDtoValidator.ValidateAsync(registerRequestDto);
        
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var result = await authService.RegisterAsync(registerRequestDto);

        return !result.IsSuccess ? Conflict(result.Error) : Ok(result.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var validationResult = await loginRequestDtoValidator.ValidateAsync(loginRequestDto);
        
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        
        var result = await authService.LoginAsync(loginRequestDto);

        return !result.IsSuccess ? Unauthorized(result.Error) : Ok(result.Value);
    }
}