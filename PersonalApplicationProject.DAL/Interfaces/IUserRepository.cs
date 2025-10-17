using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<Event>> GetUsersEventsAsync(int userId);
}