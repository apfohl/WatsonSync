using Microsoft.AspNetCore.Mvc;
using WatsonSync.Components.Attributes;
using WatsonSync.Components.DataAccess;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Authorize]
[Route("frames")]
public sealed class FramesController : ApiController
{
    private readonly IDatabase database;

    public FramesController(IDatabase database) =>
        this.database = database;

    [HttpGet]
    public async Task<IActionResult> Frames([FromQuery(Name = "last_sync")] DateTime since = default)
    {
        using var unitOfWork = database.StartUnitOfWork();

        var result = await unitOfWork.Frames.QuerySince(CurrentUser, since);

        await unitOfWork.Save();

        return Ok(result);
    }

    [HttpPost]
    [Route("bulk")]
    public async Task<IActionResult> CreateFrames([FromBody] IEnumerable<Frame> frames)
    {
        using var unitOfWork = database.StartUnitOfWork();

        await unitOfWork.Frames.InsertOrReplace(CurrentUser, frames);

        await unitOfWork.Save();

        return CreatedAtAction(nameof(Frames), null);
    }
}