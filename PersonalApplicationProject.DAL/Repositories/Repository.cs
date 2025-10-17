using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T>
    where T : class
{
    protected readonly AppDbContext Context = context;

    public async Task<T?> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
}