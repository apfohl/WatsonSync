using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;

namespace WatsonSync.Components;

public sealed class UnitOfWork : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbTransaction transaction;

    private UnitOfWork(SqliteConnection connection, DbTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public Task<IEnumerable<T>> Query<T>(string query, object parameter = null) =>
        connection.QueryAsync<T>(query, parameter, transaction);

    public Task<int> Execute(string query, object parameter = null) =>
        connection.ExecuteAsync(query, parameter, transaction);

    public Task Commit() => transaction.CommitAsync();
    
    public void Dispose()
    {
        transaction.Dispose();
        connection.Dispose();
    }

    public static async Task<UnitOfWork> Create(string connectionString)
    {
        var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        return new UnitOfWork(connection, transaction);
    }
}