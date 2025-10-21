using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.BLL.Validators.User;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be valid email address");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters");
    }
}