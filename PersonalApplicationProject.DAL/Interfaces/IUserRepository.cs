using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<Event>> GetUsersEventsAsync(int userId);
    Task<User?> GetByEmailAsync(string email);
}