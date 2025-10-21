using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class EventRepository(AppDbContext context) : Repository<Event>(context), IEventRepository
{
    public async Task<Event?> GetWithOrganizerAndParticipantsByIdAsync(int id)
    {
        return await Context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetAllWithParticipantsAsync()
    {
        return await Context.Events
            .Include(e => e.Participants)
            .ToListAsync();
    }
}