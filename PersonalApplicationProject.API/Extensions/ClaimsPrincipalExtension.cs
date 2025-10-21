using System.Security.Claims;

namespace PersonalApplicationProject.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (int.TryParse(userIdString, out var userId))
        {
            return userId;
        }
        
        return null;
    }
}