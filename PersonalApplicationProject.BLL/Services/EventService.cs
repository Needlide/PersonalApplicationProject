using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using PersonalApplicationProject.BLL.DTOs.Event;
using PersonalApplicationProject.BLL.DTOs.User;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.BLL.Validators.Event;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.BLL.Services;

public class EventService(IUnitOfWork unitOfWork)
    : IEventService
{
    public async Task<Result<IEnumerable<EventSummaryDto>>> GetAllEventsAsync()
    {
        var events = await unitOfWork.Events.GetAllWithParticipantsAsync();

        var eventSummaryDtos = events.Select(e => new EventSummaryDto
        {
            Id = e.Id, Name = e.Name, EventTimestamp = e.EventTimestamp, ParticipantCount = e.ParticipantCount,
            Capacity = e.Capacity, Visible = e.Visible
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
            Capacity = e.Capacity, Visible = e.Visible
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
            Capacity = request.Capacity,
            Visible = request.Visible
        };

        await unitOfWork.Events.AddAsync(newEvent);
        await unitOfWork.SaveChangesAsync();

        var createdEvent = await unitOfWork.Events.GetWithOrganizerAndParticipantsByIdAsync(newEvent.Id);

        if (createdEvent is null) return Result<EventDetailsDto>.Failure("Failed to create event");

        var eventDetailsDto = MapToEventDetailsDto(createdEvent);

        return Result<EventDetailsDto>.Success(eventDetailsDto);
    }

    public async Task<Result<bool>> PatchEventAsync(int eventId, JsonPatchDocument<UpdateEventRequestDto> patchDoc,
        int currentUserId)
    {
        var eventEntity = await unitOfWork.Events.GetByIdAsync(eventId);
        if (eventEntity is null) return Result<bool>.Failure("Event not found.");
        
        if (eventEntity.OrganizerId != currentUserId)
            return Result<bool>.Failure("Not authorized to update this event.");
        
        var eventToPatch = new UpdateEventRequestDto
        {
            Name = eventEntity.Name,
            Description = eventEntity.Description,
            EventTimestamp = eventEntity.EventTimestamp,
            Location = eventEntity.Location,
            Capacity = eventEntity.Capacity,
            Visible = eventEntity.Visible
        };

        if (patchDoc.Operations.Count(op => op.OperationType == OperationType.Copy)
            > 10)
        {
            throw new InvalidOperationException();
        }
        
        patchDoc.ApplyTo(eventToPatch);
        
        var validationResult = await PatchValidationHelper.ValidatePatchAsync(eventToPatch, patchDoc);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<bool>.Failure(errors);
        }

        eventEntity.Name = eventToPatch.Name;
        eventEntity.Description = eventToPatch.Description;
        eventEntity.EventTimestamp = eventToPatch.EventTimestamp;
        eventEntity.Location = eventToPatch.Location;
        eventEntity.Capacity = eventToPatch.Capacity;

        unitOfWork.Events.Update(eventEntity);
        await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteEventAsync(int eventId, int currentUserId)
    {
        var @event = await unitOfWork.Events.GetByIdAsync(eventId);

        if (@event is null || @event.OrganizerId != currentUserId) return Result<bool>.Failure("Event not found");

        unitOfWork.Events.Delete(@event);
        await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> JoinEventAsync(int eventId, int participantId)
    {
        var eventToJoin = await unitOfWork.Events.GetWithOrganizerAndParticipantsByIdAsync(eventId);

        if (eventToJoin is null) return Result<bool>.Failure("Event not found.");

        if (eventToJoin.OrganizerId == participantId)
            return Result<bool>.Failure("As the organizer, you are already attending the event.");

        var isAlreadyParticipant = eventToJoin.Participants.Any(p => p.UserId == participantId);
        if (isAlreadyParticipant) return Result<bool>.Failure("You are already registered for this event.");

        if (eventToJoin.Capacity.HasValue && eventToJoin.ParticipantCount >= eventToJoin.Capacity.Value)
            return Result<bool>.Failure("This event is at full capacity.");

        var newParticipation = new Participant
        {
            UserId = participantId,
            EventId = eventId
        };

        await unitOfWork.Participants.AddAsync(newParticipation);
        await unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> LeaveEventAsync(int eventId, int participantId)
    {
        var participation =
            (await unitOfWork.Participants.FindAsync(p => p.UserId == participantId && p.EventId == eventId
            )).FirstOrDefault();

        if (participation is null) return Result<bool>.Failure("You are not registered for this event.");

        unitOfWork.Participants.Delete(participation);
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
            Organizer = organizer,
            Participants = @event.Participants.Select(p => new UserDto { Id = p.UserId, Email = p.User.Email, FirstName = p.User.FirstName, LastName = p.User.LastName}).ToList(),
            Visible = @event.Visible
        };
    }
}