using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalApplicationProject.BLL.Interfaces;

namespace PersonalApplicationProject.Controllers;

[ApiController]
[Authorize]
[Route("api/tags")]
public class TagController(ITagService tagService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTags()
    {
        var result = await tagService.GetAllTags();
        return Ok(result.Value);
    }
}