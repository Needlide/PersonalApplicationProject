using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<IEnumerable<Event>> GetUsersEventsAsync(int userId)
    {
        return await Context.Events
            .AsNoTracking()
            .Include(e => e.Organizer)
            .Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .Where(e => e.OrganizerId == userId || e.Participants.Any(p => p.UserId == userId))
            .Include(e => e.Tags)
            .ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Event>> GetOrganizedEventsByUserIdAsync(int userId)
    {
        return await Context.Events
            .AsNoTracking()
            .Where(e => e.OrganizerId == userId)
            .Include(e => e.Organizer)
            .Include(e => e.Tags)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsWhereUserIsParticipantAsync(int userId)
    {
        return await Context.Events
            .AsNoTracking()
            .Where(e => e.Participants.Any(p => p.UserId == userId))
            .Include(e => e.Organizer)
            .Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .Include(e => e.Tags)
            .ToListAsync();
    }
}