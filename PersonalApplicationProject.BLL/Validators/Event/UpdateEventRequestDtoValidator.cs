using FluentValidation;
using PersonalApplicationProject.BLL.DTOs.Event;
using PersonalApplicationProject.BLL.DTOs.Tag;
using PersonalApplicationProject.BLL.Validators.Tag;

namespace PersonalApplicationProject.BLL.Validators.Event;

public class UpdateEventRequestDtoValidator : AbstractValidator<UpdateEventRequestDto>
{
    public UpdateEventRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(255).WithMessage("Event name cannot exceed 255 characters.");

        RuleFor(x => x.EventTimestamp)
            .NotEmpty().WithMessage("Event date and time are required.")
            .GreaterThan(DateTime.UtcNow.AddMinutes(-1))
            .WithMessage("Event must be in the future.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be a positive number.");

        RuleFor(x => x.Tags)
            .NotEmpty().WithMessage("At least one tag is required.")
            .Must(tags => tags.Count <= 5).WithMessage("Maximum 5 tags allowed.")
            .Must(BeUniqueTags).WithMessage("Duplicate tags are not allowed.")
            .ForEach(tag => tag.SetValidator(new TagDtoValidator()));
    }
    
    private static bool BeUniqueTags(ICollection<TagDto>? tags)
    {
        if (tags == null || tags.Count == 0) return true;
        
        var tagNames = tags.Select(t => t.Name?.ToLowerInvariant()).ToList();
        return tagNames.Count == tagNames.Distinct().Count();
    }
}