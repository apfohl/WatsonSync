using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public interface IUserRepository
{
    Maybe<User> FindByToken(string token);
    Maybe<User> Create(string email);
}