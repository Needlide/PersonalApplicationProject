using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.Tag;

namespace PersonalApplicationProject.BLL.Validators.Tag;

public class TagDtoValidator : AbstractValidator<TagDto>
{
    public TagDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name cannot be empty.")
            .MaximumLength(50).WithMessage("Tag name cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9-]+$").WithMessage("Tag name can only contain letters, numbers, and hyphens.");
    }
}