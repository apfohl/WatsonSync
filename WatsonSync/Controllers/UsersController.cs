using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using MonadicBits;
using WatsonSync.Components.Attributes;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

using static Functional;

[Authorize]
[Route("users")]
public sealed class UsersController : BaseController
{
    private readonly IDatabase database;

    public UsersController(IDatabase database) =>
        this.database = database;

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewUserRequest newUserRequest)
    {
        using var unitOfWork = database.StartUnitOfWork();

        var result = await (
            from emailAddress in ValidateEmailAddress(newUserRequest.Email).AsTask()
            from user in unitOfWork.Users.Create(emailAddress)
            select new NewUserResponse(user.Token));

        await unitOfWork.Save();

        return result.Match<IActionResult>(
            response => Created(string.Empty, response),
            () => StatusCode(500));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        using var unitOfWork = database.StartUnitOfWork();

        await unitOfWork.Users.Delete(CurrentUser);

        await unitOfWork.Save();

        return Ok();
    }

    private static Maybe<string> ValidateEmailAddress(string emailAddress) =>
        MailAddress.TryCreate(emailAddress, out var result) ? result.Address : Nothing;
}