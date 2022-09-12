using System.Data;
using WatsonSync.Models;
using static Dapper.SqlMapper;

namespace WatsonSync.Components.DataAccess;

public abstract class FixValueTypeHandler<T> : TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T value) =>
        parameter.Value = value;
}

public sealed class DateTimeOffsetHandler : FixValueTypeHandler<DateTimeOffset>
{
    public override DateTimeOffset Parse(object value) =>
        DateTimeOffset.Parse((string)value);
}

public sealed class GuidHandler : FixValueTypeHandler<Guid>
{
    public override Guid Parse(object value) =>
        Guid.Parse((string)value);
}

public sealed class TimeSpanHandler : FixValueTypeHandler<TimeSpan>
{
    public override TimeSpan Parse(object value) =>
        TimeSpan.Parse((string)value);
}

public sealed class UserSettingsHandler : TypeHandler<UserSettings>
{
    public override void SetValue(IDbDataParameter parameter, UserSettings value) =>
        parameter.Value = value switch
        {
            UserSettings.WorkingDays => nameof(UserSettings.WorkingDays),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

    public override UserSettings Parse(object value) =>
        Enum.TryParse((string)value, out UserSettings result)
            ? result
            : throw new ArgumentException(nameof(value));
}

public sealed class UserSettingTypesHandler : TypeHandler<UserSettingTypes>
{
    public override void SetValue(IDbDataParameter parameter, UserSettingTypes value)
    {
        parameter.Value = value switch
        {
            UserSettingTypes.Double => nameof(UserSettingTypes.Double),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public override UserSettingTypes Parse(object value) =>
        Enum.TryParse((string)value, out UserSettingTypes result)
            ? result
            : throw new ArgumentException(nameof(value));
}