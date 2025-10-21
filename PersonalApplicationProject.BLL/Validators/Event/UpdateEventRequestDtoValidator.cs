using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Validators.Event;

public class UpdateEventRequestDtoValidator : AbstractValidator<UpdateEventRequestDto>
{
    public UpdateEventRequestDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Event name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required")
            .MaximumLength(255).WithMessage("Description must not exceed 255 characters");
        RuleFor(x => x.EventTimestamp).NotEmpty().WithMessage("Event date and time are required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Event must be in the future.");
        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be a positive number.");
    }
}