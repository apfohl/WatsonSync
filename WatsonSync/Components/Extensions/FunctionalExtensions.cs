#nullable enable
using Microsoft.Extensions.Primitives;
using MonadicBits;

namespace WatsonSync.Components.Extensions;

using static Functional;

public static class FunctionalExtensions
{
    public static Maybe<T> ToMaybe<T>(this T? value) =>
        value != null ? value : Nothing;

    public static Maybe<string> ToMaybe(this StringValues value) =>
        value.Count == 0 ? Nothing : value.ToString();

    public static Func<T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 t1, T2 t2)
        => t3 => func(t1, t2, t3);
}