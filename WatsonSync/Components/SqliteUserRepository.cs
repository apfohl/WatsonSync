using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public sealed class SqliteUserRepository : IUserRepository
{
    private readonly Context context;

    public SqliteUserRepository(Context context) =>
        this.context = context;

    public Task<Maybe<User>> FindByToken(string token) =>
        Task.FromResult(new User(1, "mail@example.com", token).Just());

    public Task<Maybe<User>> Create(string email)
    {
        throw new NotImplementedException();
    }
}