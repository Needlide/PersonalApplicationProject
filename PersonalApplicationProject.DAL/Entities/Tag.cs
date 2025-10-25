using System.ComponentModel.DataAnnotations;

namespace PersonalApplicationProject.DAL.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Event> Events { get; } = new List<Event>();
}