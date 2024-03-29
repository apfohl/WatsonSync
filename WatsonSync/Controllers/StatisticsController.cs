using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WatsonSync.Components.Attributes;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Statistics;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Authorize]
[Route("statistics")]
public sealed class StatisticsController : ApiController
{
    private readonly IDatabase database;
    private readonly IOptions<Settings> options;

    public StatisticsController(IDatabase database, IOptions<Settings> options)
    {
        this.database = database;
        this.options = options;
    }

    [HttpGet]
    public async Task<IActionResult> Statistics()
    {
        await using var unitOfWork = database.StartUnitOfWork();

        var frames =
            (await unitOfWork.Frames.QuerySince(CurrentUser, default))
            .Where(frame => frame.Project != options.Value.HolidayIdentifier);

        var workingDays =
            (await unitOfWork.UserSettings.WorkingDays(CurrentUser))
            .Match(d => d, () => 8d);

        await unitOfWork.Save();

        var aggregateDailyHours = WorkTimeBalance.AggregateDailyHours(frames);
        var balance = WorkTimeBalance.CalculateBalance(workingDays, aggregateDailyHours);

        return Ok(new
        {
            balance = new
            {
                number = balance,
                @string = TimeSpan.FromHours(balance)
            }
        });
    }
}