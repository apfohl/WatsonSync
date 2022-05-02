namespace WatsonSync.Components;

public static class AsyncExtensions
{
    public static Task<T> AsTask<T>(this T value)
        => Task.FromResult(value);
}