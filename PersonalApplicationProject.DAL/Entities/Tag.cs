namespace PersonalApplicationProject.DAL.Entities;

public class Tag
{
    public string Name { get; set; }

    private ICollection<Event> Events { get; } = new List<Event>();
}