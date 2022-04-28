using System.Net.Http.Headers;
using MonadicBits;
using WatsonSync.Components;

namespace WatsonSync.Middlewares;

public sealed class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate next;

    public TokenAuthenticationMiddleware(RequestDelegate next) => this.next = next;

    public async Task Invoke(HttpContext context, UserAuthenticator userAuthenticator)
    {
        (from header in context.Request.Headers["Authorization"].ToMaybe()
                from token in AuthenticationHeaderValue.Parse(header).Parameter.ToMaybe()
                from user in userAuthenticator.Authenticate(token)
                select user)
            .Match(user => context.Items["User"] = user, () => { });

        await next(context);
    }
}