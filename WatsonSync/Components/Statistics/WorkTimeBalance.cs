using WatsonSync.Models;

namespace WatsonSync.Components.Statistics;

public static class WorkTimeBalance
{
    public static double CalculateBalance(double dailyWorkingHours, IEnumerable<double> dailyHours) =>
        dailyHours.Aggregate(0d, (balance, hours) => balance + (hours - dailyWorkingHours));

    public static IEnumerable<double> AggregateDailyHours(IEnumerable<Frame> frames)
    {
        var state = (Hours: 0d, LastDay: default(DateTime));

        foreach (var frame in frames.OrderBy(f => f.BeginAt.Date))
        {
            var hours = (frame.EndAt - frame.BeginAt).TotalHours;

            if (state.LastDay == default)
            {
                state = (hours, frame.BeginAt.Date);
                continue;
            }

            if (state.LastDay != frame.BeginAt.Date)
            {
                yield return state.Hours;
                state = (0d, frame.BeginAt.Date);
            }

            state = state with { Hours = state.Hours + hours };
        }

        yield return state.Hours;
    }
}