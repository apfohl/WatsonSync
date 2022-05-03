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
}