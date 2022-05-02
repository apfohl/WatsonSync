using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public interface IUserRepository
{
    Task<Maybe<User>> FindByToken(string token);
    Task<Maybe<User>> Create(string email);
}