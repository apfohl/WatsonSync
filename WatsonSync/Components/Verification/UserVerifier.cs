using MonadicBits;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;
using WatsonSync.Models;

namespace WatsonSync.Components.Verification;

public enum VerificationError
{
    NotFound,
    Unauthorized
}

public sealed class UserVerifier
{
    private readonly IDatabase database;

    public UserVerifier(IDatabase database) =>
        this.database = database;

    public async Task<Either<VerificationError, Token>> Verify(UserVerification userVerification)
    {
        using var unitOfWork = database.StartUnitOfWork();

        var user = await unitOfWork.Users.FindByEmail(userVerification.Email);

        var result = await user.Match(
            async u =>
            {
                if (!u.IsVerified && u.VerificationToken == userVerification.VerificationToken)
                {
                    await unitOfWork.Users.Verify(u.Email);
                    return (await unitOfWork.Users.CreateToken(u)).Right<VerificationError, Token>();
                }

                return VerificationError.Unauthorized.Left<VerificationError, Token>();
            },
            () => VerificationError.NotFound.Left<VerificationError, Token>().AsTask());

        await unitOfWork.Save();

        return result;
    }
}