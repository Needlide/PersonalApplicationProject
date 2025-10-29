using PersonalApplicationProject.BLL.DTOs.Tag;
using PersonalApplicationProject.BLL.Interfaces;
using PersonalApplicationProject.DAL.Interfaces;

namespace PersonalApplicationProject.BLL.Services;

public class TagService(IUnitOfWork unitOfWork) : ITagService
{
    public async Task<Result<IEnumerable<TagDto>>> GetAllTags()
    {
        var tags = await unitOfWork.Tags.GetAllAsync();
        var tagDtos = tags.Select(t => new TagDto { Name = t.Name });
        return Result<IEnumerable<TagDto>>.Success(tagDtos);
    }
}