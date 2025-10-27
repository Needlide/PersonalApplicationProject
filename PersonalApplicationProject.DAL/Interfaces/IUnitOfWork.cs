namespace PersonalApplicationProject.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    IParticipantRepository Participants { get; }
    ITagsRepository Tags { get; }

    Task<int> SaveChangesAsync();
}