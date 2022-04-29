using Microsoft.AspNetCore.Mvc;
using WatsonSync.Components;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Authorize]
[Route("frames")]
public sealed class FramesController : Controller
{
    private readonly IFrameRepository frameRepository;
    // private static readonly ILog Logger = LogManager.GetLogger(typeof(FramesController));

    private User CurrentUser => (User)HttpContext.Items["User"];

    public FramesController(IFrameRepository frameRepository) =>
        this.frameRepository = frameRepository;

    [HttpGet]
    public async Task<IActionResult> Frames([FromQuery(Name = "last_sync")] DateTime since) =>
        Ok(since == default
            ? await frameRepository.QueryAll(CurrentUser)
            : await frameRepository.QuerySince(CurrentUser, since));

    [HttpPost]
    [Route("bulk")]
    public async Task<IActionResult> CreateFrames([FromBody] IEnumerable<Frame> frames)
    {
        await frameRepository.Insert(CurrentUser, frames);
        return CreatedAtAction(nameof(Frames), null);
    }
}