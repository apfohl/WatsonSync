using Microsoft.AspNetCore.Mvc;
using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public static class ControllerExtensions
{
    public static Maybe<User> CurrentUser(this Controller controller)
        => ((User)controller.HttpContext.Items["User"]).ToMaybe();
}