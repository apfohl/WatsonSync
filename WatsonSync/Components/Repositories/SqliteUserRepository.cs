using System.Security.Cryptography;
using MonadicBits;
using WatsonSync.Components.DataAccess;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public sealed class SqliteUserRepository : IUserRepository
{
    private readonly Context context;

    public SqliteUserRepository(Context context) =>
        this.context = context;

    public async Task<Maybe<User>> Create(string email)
    {
        var lastId = await LastId();
        var nextId = lastId == null ? 1 : lastId.Value + 1;

        var token = CreateToken();

        await context.Execute("INSERT INTO users (id, email, token) VALUES (@Id, @Email, @Token)",
            new { Id = nextId, Email = email, Token = token });

        return new User(nextId, email, token);
    }

    private async Task<int?> LastId() =>
        await context.QuerySingle<int?>("SELECT max(id) FROM users");

    private static string CreateToken() =>
        Convert.ToHexString(RandomNumberGenerator.GetBytes(16));

}