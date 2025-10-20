using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.DTOs;

namespace PersonalApplicationProject.Serialization;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true
)]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(RegisterRequestDto))]
public partial class ApiJsonSerializerContext : JsonSerializerContext
{
}