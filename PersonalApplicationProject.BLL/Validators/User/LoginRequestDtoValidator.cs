using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.BLL.Validators.User;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be valid email address");
        RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters");
    }
}