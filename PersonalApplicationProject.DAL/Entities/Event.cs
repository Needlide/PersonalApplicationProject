using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalApplicationProject.DAL.Entities;

public class Event
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string? Description { get; set; }

    [Required] public DateTime EventTimestamp { get; set; }

    public string? Location { get; set; }

    public int? Capacity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped] public int ParticipantCount => Participants.Count;


    public int OrganizerId { get; set; }

    public User Organizer { get; set; } = null!;

    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}