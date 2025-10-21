using PersonalApplicationProject.BLL.DTOs.Event;

namespace PersonalApplicationProject.BLL.Interfaces;

public interface IEventService
{
    Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsAsync();

    Task<Result<EventDetailsDto?>> GetEventDetailsAsync(int id);
    Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsForUserAsync(int userId);

    Task<Result<EventDetailsDto>> CreateEventAsync(CreateEventRequestDto request, int organizerId);

    Task<Result<EventDetailsDto>> UpdateEventAsync(int eventId, UpdateEventRequestDto request, int currentUserId);

    Task<Result<bool>> DeleteEventAsync(int eventId, int currentUserId);
}