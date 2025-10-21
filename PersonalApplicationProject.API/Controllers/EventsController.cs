using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.BLL.DTOs.Event;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.Extensions;

namespace PersonalApplicationProject.Controllers;

[Authorize]
[ApiController]
[Route("api/events")]
public class EventsController(IEventService eventService) : ControllerBase
{
    private const string InvalidUserToken = "Invalid user token";

    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var result = await eventService.GetAllEventsAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        var result = await eventService.GetEventDetailsAsync(id);

        return result.Value is null ? NotFound() : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequestDto createEventRequestDto)
    {
        var organizerId = User.GetUserId();

        if (organizerId is null) return Unauthorized(InvalidUserToken);

        var result = await eventService.CreateEventAsync(createEventRequestDto, organizerId.Value);

        if (!result.IsSuccess) return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetEventById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchEvent(int id, [FromBody] JsonPatchDocument<UpdateEventRequestDto>? patchDoc)
    {
        var currentUserId = User.GetUserId();

        if (currentUserId is null) return Unauthorized(InvalidUserToken);

        if (patchDoc is null) return BadRequest("A patch document is required.");

        var result = await eventService.PatchEventAsync(id, patchDoc, currentUserId.Value);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var currentUserId = User.GetUserId();

        if (currentUserId is null) return Unauthorized(InvalidUserToken);

        var result = await eventService.DeleteEventAsync(id, currentUserId.Value);

        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    [HttpPost("{id:int}/join")]
    public async Task<IActionResult> JoinEvent(int id)
    {
        var participantId = User.GetUserId();

        if (participantId is null) return Unauthorized(InvalidUserToken);

        var result = await eventService.JoinEventAsync(id, participantId.Value);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:int}/leave")]
    public async Task<IActionResult> LeaveEvent(int id)
    {
        var participantId = User.GetUserId();

        if (participantId is null) return Unauthorized(InvalidUserToken);

        var result = await eventService.LeaveEventAsync(id, participantId.Value);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}