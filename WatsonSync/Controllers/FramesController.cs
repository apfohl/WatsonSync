using Microsoft.AspNetCore.Mvc;
using WatsonSync.Components;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Authorize]
[Route("frames")]
public sealed class FramesController : Controller
{
    private readonly UnitOfWork unitOfWork;
    // private static readonly ILog Logger = LogManager.GetLogger(typeof(FramesController));

    public FramesController(IContextFactory contextFactory) =>
        unitOfWork = new UnitOfWork(contextFactory);

    private User CurrentUser => (User)HttpContext.Items["User"];

    [HttpGet]
    public async Task<IActionResult> Frames([FromQuery(Name = "last_sync")] DateTime since)
    {
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
        await unitOfWork.FrameRepository.Insert(CurrentUser, frames);
        
        await unitOfWork.Save();
        
        return CreatedAtAction(nameof(Frames), null);
    }

    protected override void Dispose(bool disposing)
    {
        unitOfWork.Dispose();
        base.Dispose(disposing);
    }
}