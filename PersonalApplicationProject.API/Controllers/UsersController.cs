using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.Extensions;

namespace PersonalApplicationProject.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController(IEventService eventService) : ControllerBase
{
    [HttpGet("me/events")]
    public async Task<IActionResult> GetUsersEvents()
    {
        var organizerId = User.GetUserId();
        
        if (organizerId is null) return Unauthorized("Invalid user token.");

        var result = await eventService.GetAllEventsForUserAsync(organizerId.Value);
        
        return Ok(result.Value);
    }
}