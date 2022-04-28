using log4net;
using Microsoft.AspNetCore.Mvc;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Route("frames")]
public sealed class FramesController : Controller
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(FramesController));
    
    [HttpGet]
    public IActionResult Frames([FromQuery(Name = "last_sync")] DateTime since)
    {
        Logger.Info("Query Frames");

        return Ok(Enumerable.Empty<Frame>());
    }

    [HttpPost]
    [Route("bulk")]
    public IActionResult CreateFrames([FromBody] IEnumerable<Frame> frames)
    {
        Logger.InfoFormat("Create Frames: {0}", frames);

        return CreatedAtAction(nameof(Frames), null);
    }
}