using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.BLL.Validators.User;

public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token must not be empty");
    }
}