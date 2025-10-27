using PersonalApplicationProject.BLL.DTOs.Tag;
using PersonalApplicationProject.BLL.DTOs.User;

namespace PersonalApplicationProject.BLL.DTOs.Event;

public class EventDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EventTimestamp { get; set; }
    public string? Location { get; set; }
    public int? Capacity { get; set; }
    public bool Visible { get; set; }
    public int ParticipantCount { get; set; }

    public UserDto Organizer { get; set; } = null!;
    public IEnumerable<UserDto>? Participants { get; set; }
    public IEnumerable<TagDto> Tags { get; set; } = new List<TagDto>();
}