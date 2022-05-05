using System.Security.Cryptography;
using MonadicBits;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public sealed class SqliteUserRepository : IUserRepository
{
    private readonly Context context;

    public SqliteUserRepository(Context context) =>
        this.context = context;

    public async Task<Maybe<User>> FindByEmail(string email) =>
        (await context.QuerySingleOrDefault<User>(
            "SELECT id AS Id, email AS Email, token AS Token, verification_token AS VerificationToken, is_verified AS IsVerified FROM users WHERE email is @Email",
            new { Email = email }))
        .ToMaybe();

    public async Task<Maybe<VerificationToken>> Create(string email)
    {
        var lastId = await LastId();
        var nextId = lastId == null ? 1 : lastId.Value + 1;

        var token = CreateToken();

        await context.Execute(
            "INSERT INTO users (id, email, verification_token, is_verified) VALUES (@Id, @Email, @VerificationToken, @IsVerified)",
            new { Id = nextId, Email = email, VerificationToken = token.Value, IsVerified = false });

        return new VerificationToken(token.Value);
    }

    public Task Delete(User user) =>
        context.Execute("DELETE FROM users WHERE id IS @Id", user);

    public async Task Verify(string email) =>
        await context.Execute("UPDATE users SET is_verified = @IsVerified WHERE email is @Email",
            new { IsVerified = true, Email = email });

    public async Task<Token> CreateToken(User user)
    {
        var token = CreateToken();

        await context.Execute(
            "UPDATE users SET token = @Token WHERE id IS @Id",
            new { Token = token.Value, user.Id });

        return token;
    }

    private async Task<int?> LastId() =>
        await context.QuerySingle<int?>("SELECT max(id) FROM users");

    private static Token CreateToken() =>
        new(Convert.ToHexString(RandomNumberGenerator.GetBytes(16)));
}