namespace WatsonSync.Models;

public enum UserSettings
{
    WorkingDays
}

public enum UserSettingTypes
{
    Double
}

public sealed record UserSetting
{
    public UserSettings Key { get; set; }
    public string Value { get; set; }
    public UserSettingTypes Type { get; set; }

    public UserSetting()
    {
    }

    public UserSetting(UserSettings key, string value, UserSettingTypes type)
    {
        Key = key;
        Value = value;
        Type = type;
    }
}

