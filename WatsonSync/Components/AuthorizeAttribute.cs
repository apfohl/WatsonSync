using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WatsonSync.Models;

namespace WatsonSync.Components;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }
        
        var user = ((User)context.HttpContext.Items["User"]).ToMaybe();
        user.Match(
            _ => { },
            () => context.Result = new JsonResult(null)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            });
    }
}