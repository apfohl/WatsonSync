using System.Security.Cryptography;
using System.Text;
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

    public async Task<Maybe<User>> Create(string email)
    {
        var lastId = await LastId();
        var nextId = lastId == null ? 1 : lastId.Value + 1;

        var (token, hash) = CreateToken(email);
        await context.Execute("INSERT INTO users (id, email, token) VALUES (@Id, @Email, @Hash)",
            new { Id = nextId, Email = email, Hash = hash });

        return new User(nextId, email, token);
    }

    private async Task<int?> LastId() =>
        await context.QuerySingle<int?>("SELECT max(id) FROM users");

    private static (string Token, string Hash) CreateToken(string salt)
    {
        var guid = Guid.NewGuid();
        var tokenBytes = guid.ToByteArray();
        var saltBytes = Encoding.ASCII.GetBytes(salt);

        HashAlgorithm algorithm = SHA256.Create();

        var plainTextWithSaltBytes = new byte[tokenBytes.Length + salt.Length];

        for (var i = 0; i < tokenBytes.Length; i++)
        {
            plainTextWithSaltBytes[i] = tokenBytes[i];
        }

        for (var i = 0; i < salt.Length; i++)
        {
            plainTextWithSaltBytes[tokenBytes.Length + i] = saltBytes[i];
        }

        var hash = algorithm.ComputeHash(plainTextWithSaltBytes);

        return (guid.ToString(), Convert.ToBase64String(hash));
    }
}