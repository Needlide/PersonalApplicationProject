using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Validators.Event;

public class CreateEventRequestDtoValidator : AbstractValidator<CreateEventRequestDto>
{
    public CreateEventRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(255).WithMessage("Event name cannot exceed 255 characters.");

        RuleFor(x => x.EventTimestamp)
            .NotEmpty().WithMessage("Event date and time are required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Event must be in the future.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be a positive number.");
    }
}