namespace PersonalApplicationProject.DAL.Entities;

public class Participant
{
    public int UserId { get; set; }
    public int EventId { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Event Event { get; set; } = null!;
}