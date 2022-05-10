using System.Data;
using static Dapper.SqlMapper;

namespace WatsonSync.Components.DataAccess;

public abstract class SqliteTypeHandler<T> : TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T value) =>
        parameter.Value = value;
}

public sealed class DateTimeOffsetHandler : SqliteTypeHandler<DateTimeOffset>
{
    public override DateTimeOffset Parse(object value) =>
        DateTimeOffset.Parse((string)value);
}

public sealed class GuidHandler : SqliteTypeHandler<Guid>
{
    public override Guid Parse(object value) =>
        Guid.Parse((string)value);
}

public sealed class TimeSpanHandler : SqliteTypeHandler<TimeSpan>
{
    public override TimeSpan Parse(object value) =>
        TimeSpan.Parse((string)value);
}