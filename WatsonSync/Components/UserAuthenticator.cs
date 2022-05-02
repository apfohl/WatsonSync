using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public sealed class UserAuthenticator : IDisposable
{
    private readonly UnitOfWork unitOfWork;

    public UserAuthenticator(IContextFactory contextFactory) =>
        unitOfWork = new UnitOfWork(contextFactory);

    public async Task<Maybe<User>> Authenticate(string token)
    {
        var user = await unitOfWork.UserRepository.FindByToken(token);
        await unitOfWork.Save();
        return user;
    }

    public void Dispose() => 
        unitOfWork.Dispose();
}