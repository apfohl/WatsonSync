using System.Net.Http.Headers;
using MonadicBits;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;

namespace WatsonSync.Components.Authentication;

public sealed class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate next;

    public TokenAuthenticationMiddleware(RequestDelegate next) =>
        this.next = next;

    public async Task Invoke(HttpContext context, IDatabase database)
    {
        (await (from header in context.Request.Headers["Authorization"].ToMaybe().AsTask()
                from token in AuthenticationHeaderValue.Parse(header).Parameter.ToMaybe().AsTask()
                from user in database.FindUserByToken(token)
                select user))
            .Match(user => context.Items["User"] = user, () => { });

        await next(context);
    }
}