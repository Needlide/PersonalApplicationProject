using PersonalApplicationProject.BLL.DTOs.Tag;

namespace PersonalApplicationProject.BLL.DTOs.Event;

public class UpdateEventRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventTimestamp { get; set; }

    public string? Location { get; set; }

    public int? Capacity { get; set; }
    
    public bool Visible { get; set; }
    public ICollection<TagDto> Tags { get; set; } = new List<TagDto>();
}