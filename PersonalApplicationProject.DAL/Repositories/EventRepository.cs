using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class EventRepository(AppDbContext context) : Repository<Event>(context), IEventRepository
{
    
}