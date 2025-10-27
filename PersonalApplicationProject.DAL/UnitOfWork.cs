using PersonalApplicationProject.DAL.Interfaces;
using PersonalApplicationProject.DAL.Repositories;

namespace PersonalApplicationProject.DAL;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IUserRepository Users { get; } = new UserRepository(context);
    public IEventRepository Events { get; } = new EventRepository(context);
    public IParticipantRepository Participants { get; } = new ParticipantRepository(context);
    public ITagsRepository Tags { get; } = new TagsRepository(context);

    public void Dispose()
    {
        context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}