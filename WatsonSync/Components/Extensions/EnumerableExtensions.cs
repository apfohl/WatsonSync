namespace WatsonSync.Components.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var e in enumerable)
        {
            action(e);
        }
    }

    public static async Task ForEach<T>(this IEnumerable<T> enumerable, Func<T, Task> func)
    {
        foreach (var e in enumerable)
        {
            await func(e);
        }
    }
}