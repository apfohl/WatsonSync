using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MonadicBits;
using WatsonSync.Components.Attributes;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;
using WatsonSync.Components.Mailing;
using WatsonSync.Components.Validation;
using WatsonSync.Components.Verification;
using WatsonSync.Models;

namespace WatsonSync.Controllers;

[Authorize]
[Route("users")]
public sealed class UsersController : ApiController
{
    private readonly IDatabase database;
    private readonly IMailer mailer;
    private readonly IOptions<MailSettings> options;
    private readonly UserVerifier userVerifier;

    public UsersController(IDatabase database, UserVerifier userVerifier, IMailer mailer,
        IOptions<MailSettings> options)
    {
        this.database = database;
        this.userVerifier = userVerifier;
        this.mailer = mailer;
        this.options = options;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NewUser newUser)
    {
        await using var unitOfWork = database.StartUnitOfWork();

        var result = await (
            from emailAddress in PropertyValidation.ValidateEmailAddress(newUser.Email).AsTask()
            from verificationToken in unitOfWork.Users.Create(emailAddress)
            select (Email: emailAddress, VerificationToken: verificationToken.Value));

        await unitOfWork.Save();

        return await result.Match<Task<IActionResult>>(
            async r =>
            {
                await mailer.Send(
                    r.Email,
                    "Activation required",
                    $"Please follow the link to activate your account: {options.Value.ApplicationUrl}/users/verification" +
                    $"?email={r.Email}&verificationToken={r.VerificationToken}");

                return Created(string.Empty, null);
            },
            () => StatusCode(500).AsTask<IActionResult>());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        await using var unitOfWork = database.StartUnitOfWork();

        await unitOfWork.Users.Delete(CurrentUser);

        await unitOfWork.Save();

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("verify")]
    public async Task<IActionResult> Verify([FromBody] UserVerification userVerification) =>
        (await userVerifier.Verify(userVerification))
        .Match(
            error => error switch
            {
                VerificationError.NotFound => NotFound(),
                VerificationError.Unauthorized => (IActionResult)Unauthorized(),
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
            },
            token => new CreatedResult(string.Empty, token));
}