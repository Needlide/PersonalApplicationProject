using PersonalApplicationProject.BLL.DTOs.Tag;

namespace PersonalApplicationProject.BLL.Interfaces;

public interface ITagService
{
    Task<Result<IEnumerable<TagDto>>> GetAllTags();
}