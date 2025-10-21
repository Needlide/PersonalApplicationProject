using PersonalApplicationProject.BLL.DTOs.Event;
using PersonalApplicationProject.BLL.DTOs.User;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.BLL.Services;

public class EventService(IUnitOfWork unitOfWork) : IEventService
{
    public async Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsAsync()
    {
        var events = await unitOfWork.Events.GetAllWithParticipantsAsync();

        var eventSummaryDtos = events.Select(e => new EventSummaryDto
        {
            Id = e.Id, Name = e.Name, EventTimestamp = e.EventTimestamp, ParticipantCount = e.ParticipantCount,
            Capacity = e.Capacity
        });

        return Result<IEnumerable<EventSummaryDto>>.Success(eventSummaryDtos);
    }

    public async Task<Result<EventDetailsDto?>> GetEventDetailsAsync(int id)
    {
        var @event = await unitOfWork.Events.GetWithOrganizerAndParticipantsByIdAsync(id);

        if (@event is null) return Result<EventDetailsDto?>.Failure("Event not found");

        var eventDetails = MapToEventDetailsDto(@event);

        return Result<EventDetailsDto?>.Success(eventDetails);
    }

    public async Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsForUserAsync(int userId)
    {
        var usersEvents = await unitOfWork.Users.GetUsersEventsAsync(userId);

        var eventSummaryDtos = usersEvents.Select(e => new EventSummaryDto
        {
            Id = e.Id, Name = e.Name, EventTimestamp = e.EventTimestamp, ParticipantCount = e.ParticipantCount,
            Capacity = e.Capacity
        });

        return Result<IEnumerable<EventSummaryDto>>.Success(eventSummaryDtos);
    }

    public async Task<Result<EventDetailsDto>> CreateEventAsync(CreateEventRequestDto request, int organizerId)
    {
        var newEvent = new Event
        {
            OrganizerId = organizerId,
            Name = request.Name,
            Description = request.Description,
            EventTimestamp = request.EventTimestamp,
            Location = request.Location,
            Capacity = request.Capacity
        };

        await unitOfWork.Events.AddAsync(newEvent);
        await unitOfWork.SaveChangesAsync();

        var createdEvent = await unitOfWork.Events.GetWithOrganizerAndParticipantsByIdAsync(newEvent.Id);

        if (createdEvent is null) return Result<EventDetailsDto>.Failure("Failed to create event");

        var eventDetailsDto = MapToEventDetailsDto(createdEvent);

        return Result<EventDetailsDto>.Success(eventDetailsDto);
    }

    public async Task<Result<EventDetailsDto>> UpdateEventAsync(int eventId, UpdateEventRequestDto request,
        int currentUserId)
    {
        var @event = await unitOfWork.Events.GetByIdAsync(eventId);

        if (@event is null || @event.OrganizerId != currentUserId)
            return Result<EventDetailsDto>.Failure("Event not found or you do not have permission to edit it");

        @event.Name = request.Name;
        @event.Description = request.Description;
        @event.EventTimestamp = request.EventTimestamp;
        @event.Location = request.Location;
        @event.Capacity = request.Capacity;

        unitOfWork.Events.Update(@event);
        await unitOfWork.SaveChangesAsync();

        var eventDetails = MapToEventDetailsDto(@event);

        return Result<EventDetailsDto>.Success(eventDetails);
    }

    public async Task<Result<bool>> DeleteEventAsync(int eventId, int currentUserId)
    {
        var @event = await unitOfWork.Events.GetByIdAsync(eventId);

        if (@event is null || @event.OrganizerId != currentUserId) return Result<bool>.Failure("Event not found");

        unitOfWork.Events.Delete(@event);
        await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    private static EventDetailsDto MapToEventDetailsDto(Event @event)
    {
        var organizer = new UserDto
        {
            Id = @event.OrganizerId,
            Email = @event.Organizer.Email,
            FirstName = @event.Organizer.FirstName,
            LastName = @event.Organizer.LastName
        };

        return new EventDetailsDto
        {
            Id = @event.Id,
            Name = @event.Name,
            Description = @event.Description,
            EventTimestamp = @event.EventTimestamp,
            Location = @event.Location,
            Capacity = @event.Capacity,
            ParticipantCount = @event.ParticipantCount,
            Organizer = organizer
        };
    }
}