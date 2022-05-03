using Dapper;
using Microsoft.Data.Sqlite;

namespace WatsonSync.Models;

public sealed class ReadOnlyContext : IDisposable
{
    private readonly SqliteConnection connection;

    public ReadOnlyContext(SqliteConnection connection) =>
        this.connection = connection;

    public Task<T> QuerySingleOrDefault<T>(string query, object parameter = null) =>
        connection.QuerySingleOrDefaultAsync<T>(query, parameter);

    public void Dispose()
    {
        connection.Close();
        connection.Dispose();
    }
}