using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Validators.Event;

public class UpdateEventRequestDtoValidator : AbstractValidator<UpdateEventRequestDto>
{
    public UpdateEventRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(255);

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .When(x => x.Description != null);

        RuleFor(x => x.EventTimestamp)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow).WithMessage("Event must be in the future.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue);
    }
}