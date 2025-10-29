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

    [HttpGet("me/organized")]
    public async Task<IActionResult> GetEventsOrganizedByUser()
    {
        var organizerId = User.GetUserId();

        if (organizerId is null) return Unauthorized("Invalid user token.");

        var result = await eventService.GetAllEventsWhereUserIsOrganizerAsync(organizerId.Value);

        return Ok(result.Value);
    }

    [HttpGet("me/participating")]
    public async Task<IActionResult> GetEventsWhereUserIsParticipant()
    {
        var userId = User.GetUserId();

        if (userId is null) return Unauthorized("Invalid user token.");

        var result = await eventService.GetAllEventsWhereUserIsParticipantAsync(userId.Value);

        return Ok(result.Value);
    }
}