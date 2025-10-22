namespace PersonalApplicationProject.BLL.DTOs.Event;

public class EventSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime EventTimestamp { get; set; }
    public int ParticipantCount { get; set; }
    public int? Capacity { get; set; }
    public bool Visible { get; set; }
}