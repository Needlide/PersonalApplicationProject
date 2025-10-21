using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.BLL.DTOs;
using PersonalApplicationProject.BLL.DTOs.Event;
using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.Serialization;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true
)]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(RegisterRequestDto))]
[JsonSerializable(typeof(LoginResponseDto))]
[JsonSerializable(typeof(LoginRequestDto))]
[JsonSerializable(typeof(CreateEventRequestDto))]
[JsonSerializable(typeof(EventDetailsDto))]
[JsonSerializable(typeof(EventSummaryDto))]
[JsonSerializable(typeof(UpdateEventRequestDto))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}