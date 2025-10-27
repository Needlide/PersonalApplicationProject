using Microsoft.EntityFrameworkCore;
using PersonalApplicationProject.DAL.Entities;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.DAL.Repositories;

public class TagsRepository(AppDbContext context) : Repository<Tag>(context), ITagsRepository
{
    public async Task<IEnumerable<Tag>> GetTagsByNamesAsync(IEnumerable<string> names)
    {
        var normalizedNames = names.Select(n => n.ToLowerInvariant()).ToList();
        return await Context.Tags
            .Where(t => normalizedNames.Contains(t.Name))
            .ToListAsync();
    }
}