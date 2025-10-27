using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<IEnumerable<Event>> GetUsersEventsAsync(int userId)
    {
        return await Context.Events.Where(e => e.OrganizerId == userId).Include(e => e.Tags).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}