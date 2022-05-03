using Microsoft.AspNetCore.Mvc;
using WatsonSync.Components.Attributes;
using WatsonSync.Components.DataAccess;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Authorize]
[Route("frames")]
public sealed class FramesController : Controller
{
    private readonly IDatabase database;
    // private static readonly ILog Logger = LogManager.GetLogger(typeof(FramesController));

    public FramesController(IDatabase database) => 
        this.database = database;

    private User CurrentUser => (User)HttpContext.Items["User"];

    [HttpGet]
    public async Task<IActionResult> Frames([FromQuery(Name = "last_sync")] DateTime since)
    {
        using var unitOfWork = database.StartUnitOfWork();
        
        var result = since == default
            ? await unitOfWork.FrameRepository.QueryAll(CurrentUser)
            : await unitOfWork.FrameRepository.QuerySince(CurrentUser, since);
        
        await unitOfWork.Save();

        return Ok(result);
    }

    [HttpPost]
    [Route("bulk")]
    public async Task<IActionResult> CreateFrames([FromBody] IEnumerable<Frame> frames)
    {
        using var unitOfWork = database.StartUnitOfWork();
        
        await unitOfWork.FrameRepository.Insert(CurrentUser, frames);
        
        await unitOfWork.Save();
        
        return CreatedAtAction(nameof(Frames), null);
    }
}