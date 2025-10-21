using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class ParticipantRepository(AppDbContext context) : Repository<Participant>(context), IParticipantRepository
{
    
}