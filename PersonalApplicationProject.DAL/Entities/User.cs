using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalApplicationProject.DAL.Entities;

public class User
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(255)] public string Email { get; set; }

    [Required] public string PasswordHash { get; set; }

    [Required] [MaxLength(100)] public string FirstName { get; set; }

    [Required] [MaxLength(100)] public string LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [InverseProperty(nameof(Event.Organizer))]
    public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();

    public ICollection<Participant> Participations { get; set; } = new List<Participant>();
}