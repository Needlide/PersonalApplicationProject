using PersonalApplicationProject.DAL.Entities;

namespace PersonalApplicationProject.DAL.Interfaces;

public interface ITagsRepository : IRepository<Tag>
{
    Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> names);
}