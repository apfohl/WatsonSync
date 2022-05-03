using System.Data;
using Microsoft.Data.Sqlite;

namespace WatsonSync.Components.DataAccess;

public sealed class SqliteContextFactory : IContextFactory
{
    private readonly string connectionString;

    public SqliteContextFactory(string connectionString) =>
        this.connectionString = connectionString;

    public Context Create()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);

        return new Context(connection, transaction);
    }

    public ReadOnlyContext CreateReadOnly()
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        
        return new ReadOnlyContext(connection);
    }
}