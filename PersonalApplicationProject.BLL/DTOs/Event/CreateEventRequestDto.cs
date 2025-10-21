namespace PersonalApplicationProject.BLL.DTOs.Event;

public class CreateEventRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventTimestamp { get; set; }

    public string? Location { get; set; }

    public int? Capacity { get; set; }
}