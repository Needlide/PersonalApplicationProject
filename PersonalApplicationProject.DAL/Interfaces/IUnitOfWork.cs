namespace PersonalApplicationProject.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    
    
    Task<int> SaveChangesAsync();
}