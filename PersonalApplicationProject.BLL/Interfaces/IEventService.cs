using Microsoft.AspNetCore.JsonPatch;
using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Interfaces;

public interface IEventService
{
    Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsAsync();

    Task<Result<EventDetailsDto?>> GetEventDetailsAsync(int id);
    Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsForUserAsync(int userId);
    Task<Result<IEnumerable<EventDetailsDto>>> GetAllEventsWhereUserIsParticipantAsync(int userId);
    Task<Result<IEnumerable<EventDetailsDto>>> GetAllEventsWhereUserIsOrganizerAsync(int userId);

    Task<Result<EventDetailsDto>> CreateEventAsync(CreateEventRequestDto request, int organizerId);

    Task<Result<bool>> PatchEventAsync(int eventId, JsonPatchDocument<UpdateEventRequestDto> patchDoc,
        int currentUserId);

    Task<Result<bool>> DeleteEventAsync(int eventId, int currentUserId);

    Task<Result<bool>> JoinEventAsync(int eventId, int participantId);
    Task<Result<bool>> LeaveEventAsync(int eventId, int participantId);
}