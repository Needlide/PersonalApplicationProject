using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Interfaces;

public interface IPatchEventValidatorFactory
{
    IValidator<UpdateEventRequestDto> CreateValidator(JsonPatchDocument<UpdateEventRequestDto> patchDoc);
}