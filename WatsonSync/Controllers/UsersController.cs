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
public sealed class UsersController : ApiController
{
    private readonly IDatabase database;

    public UsersController(IDatabase database) =>
        this.database = database;

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewUser newUser)
    {
        using var unitOfWork = database.StartUnitOfWork();

        var result = await (
            from emailAddress in ValidateEmailAddress(newUser.Email).AsTask()
            from verificationToken in unitOfWork.Users.Create(emailAddress)
            select new UserCreated(verificationToken.Value));

        await unitOfWork.Save();

        return result.Match<IActionResult>(
            _ => Created(string.Empty, null),
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

    [AllowAnonymous]
    [HttpPost]
    [Route("verify")]
    public async Task<IActionResult> Verify([FromBody] UserVerification userVerification)
    {
        using var unitOfWork = database.StartUnitOfWork();

        var user = await unitOfWork.Users.FindByEmail(userVerification.Email);

        var response = await user.Match(
            async u =>
            {
                if (!u.IsVerified && u.VerificationToken == userVerification.VerificationToken)
                {
                    await unitOfWork.Users.Verify(u.Email);
                    var token = await unitOfWork.Users.CreateToken(u);
                    return Created(string.Empty, new { Token = token.Value });
                }

                return new UnauthorizedResult();
            },
            () => Task.FromResult((IActionResult)NotFound()));

        await unitOfWork.Save();

        return response;
    }

    private static Maybe<string> ValidateEmailAddress(string emailAddress) =>
        MailAddress.TryCreate(emailAddress, out var result) ? result.Address : Nothing;
}