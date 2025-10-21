using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<Event?> GetWithOrganizerAndParticipantsByIdAsync(int id);
    Task<IEnumerable<Event>> GetAllWithParticipantsAsync();
}