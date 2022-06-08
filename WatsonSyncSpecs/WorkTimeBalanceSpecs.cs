using WatsonSync.Models;

namespace WatsonSyncSpec;

public static class WorkTimeBalanceSpecs
{
    [Test]
    public static void AggregateDailyHours_calculates_hourly_work_time_for_every_day_in_sequence()
    {
        var frames = new[]
        {
            Frame(DateTime.Parse("2022-01-02T10:00"), DateTime.Parse("2022-01-02T12:00")),
            Frame(DateTime.Parse("2022-01-01T08:00"), DateTime.Parse("2022-01-01T12:00")),
            Frame(DateTime.Parse("2022-01-01T12:30"), DateTime.Parse("2022-01-01T16:00")),
            Frame(DateTime.Parse("2022-01-02T12:45"), DateTime.Parse("2022-01-02T17:00")),
            Frame(DateTime.Parse("2022-01-02T08:00"), DateTime.Parse("2022-01-02T09:30")),
            Frame(DateTime.Parse("2022-01-03T08:00"), DateTime.Parse("2022-01-03T14:30"))
        };

        var result = WorkTimeBalance.AggregateDailyHours(frames);

        Assert.That(result, Is.EqualTo(new[] { 7.5d, 7.75d, 6.5d }));
    }

    [Test]
    public static void CalculateBalance_calculates_work_time_balance_of_given_daily_hours()
    {
        var dailyHours = new[] { 7.5d, 7.75d, 6.5d };

        var result = WorkTimeBalance.CalculateBalance(7d, dailyHours);

        Assert.That(result, Is.EqualTo(0.75d));
    }

    private static Frame Frame(DateTime start, DateTime end) =>
        new(Guid.Empty, start, end, string.Empty, Enumerable.Empty<string>());
}