using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;

namespace WatsonSync.Components.DataAccess;

public sealed class Context : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbTransaction transaction;

    public Context(SqliteConnection connection, DbTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public Task<IEnumerable<T>> Query<T>(string query, object parameter = null) =>
        connection.QueryAsync<T>(query, parameter, transaction);

    public Task<int> Execute(string query, object parameter = null) =>
        connection.ExecuteAsync(query, parameter, transaction);

    public Task<T> QuerySingle<T>(string query, object parameter = null) =>
        connection.QuerySingleAsync<T>(query, parameter, transaction);

    public Task Commit() =>
        transaction.CommitAsync();
    
    public void Dispose()
    {
        transaction.Dispose();
        connection.Close();
        connection.Dispose();
    }
}