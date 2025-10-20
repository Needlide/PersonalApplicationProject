using Microsoft.Extensions.Options;

namespace PersonalApplicationProject.Options.Validations;

public class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        var validationErrors = new List<string>();

        if (string.IsNullOrEmpty(options.Issuer))
        {
            validationErrors.Add("JWT Issuer is required.");
        }

        if (string.IsNullOrEmpty(options.Audience))
        {
            validationErrors.Add("JWT Audience is required.");
        }

        if (string.IsNullOrEmpty(options.Key))
        {
            validationErrors.Add("JWT Key is required.");
        }
        else if (options.Key.Length < 32)
        {
            validationErrors.Add("The JWT Key must be at least 32 characters long for security.");
        }

        return validationErrors.Count != 0 ? ValidateOptionsResult.Fail(validationErrors) : ValidateOptionsResult.Success;
    }
}