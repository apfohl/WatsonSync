using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public interface IUserRepository
{
    Task<Maybe<User>> FindByEmail(string email);
    Task<Maybe<VerificationToken>> Create(string email);
    Task Delete(User user);
    Task Verify(string email);
}