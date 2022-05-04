using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public interface IUserRepository
{
    Task<Maybe<User>> Create(string email);
    Task Delete(User user);
}