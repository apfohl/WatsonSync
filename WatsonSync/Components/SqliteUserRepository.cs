using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public sealed class SqliteUserRepository : IUserRepository
{
    public Maybe<User> FindByToken(string token) =>
        new User(1, token);

    public Maybe<User> Create(string email) =>
        new User(42, Guid.NewGuid().ToString());
}