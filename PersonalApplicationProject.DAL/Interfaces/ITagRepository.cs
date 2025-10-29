using System.Linq.Expressions;
using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> names);
}