namespace PersonalApplicationProject.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    IParticipantRepository Participants { get; }
    ITagRepository Tags { get; }

    Task<int> SaveChangesAsync();
}