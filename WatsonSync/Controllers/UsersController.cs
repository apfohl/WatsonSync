using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using MonadicBits;
using WatsonSync.Components;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

using static Functional;

// [Authorize]
[Route("users")]
public sealed class UsersController : Controller
{
    private readonly UnitOfWork unitOfWork;

    public UsersController(IContextFactory contextFactory) =>
        unitOfWork = new UnitOfWork(contextFactory);

    // [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewUserRequest newUserRequest)
    {
        var result = await (
            from emailAddress in ValidateEmailAddress(newUserRequest.Email).AsTask()
            from user in unitOfWork.UserRepository.Create(emailAddress)
            select new NewUserResponse(user.Token));

        await unitOfWork.Save();

        return result.Match<IActionResult>(
            response => Created(string.Empty, response),
            () => StatusCode(500));
    }

    protected override void Dispose(bool disposing)
    {
        unitOfWork.Dispose();
        base.Dispose(disposing);
    }

    private static Maybe<string> ValidateEmailAddress(string emailAddress) =>
        MailAddress.TryCreate(emailAddress, out var result) ? result.Address : Nothing;
}