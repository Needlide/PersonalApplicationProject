using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Validators.Event;

public static class PatchValidationHelper
{
    public static async Task<ValidationResult> ValidatePatchAsync(
        UpdateEventRequestDto dto,
        JsonPatchDocument<UpdateEventRequestDto> patchDoc)
    {
        var modifiedPaths = patchDoc.Operations
            .Select(op => op.path.TrimStart('/').ToLower())
            .ToHashSet();

        var validator = new InlineValidator<UpdateEventRequestDto>();

        if (modifiedPaths.Contains("name"))
            validator.RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Event name is required.")
                .MaximumLength(255);

        if (modifiedPaths.Contains("eventtimestamp"))
            validator.RuleFor(x => x.EventTimestamp)
                .GreaterThan(DateTime.UtcNow).WithMessage("Event must be in the future.");

        if (modifiedPaths.Contains("capacity"))
            validator.RuleFor(x => x.Capacity)
                .GreaterThan(0);

        return await validator.ValidateAsync(dto);
    }
}