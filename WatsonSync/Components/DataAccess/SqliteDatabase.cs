using MonadicBits;
using WatsonSync.Components.Extensions;
using WatsonSync.Models;

namespace WatsonSync.Components.DataAccess;

public sealed class SqliteDatabase : IDatabase
{
    private readonly IContextFactory contextFactory;

    public SqliteDatabase(IContextFactory contextFactory) =>
        this.contextFactory = contextFactory;

    public UnitOfWork StartUnitOfWork() =>
        new(contextFactory);

    public async Task<Maybe<User>> FindUserByToken(string token)
    {
        using var context = contextFactory.CreateReadOnly();

        var user = await context.QuerySingleOrDefault<User>(
            "SELECT id AS Id, email AS Email, token AS Token FROM users WHERE token IS @Token",
            new { Token = token });

        return user.ToMaybe();
    }
}