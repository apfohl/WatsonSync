using MonadicBits;
using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public sealed class SqliteUserSettingsRepository : IUserSettingsRepository
{
    private readonly Context context;

    public SqliteUserSettingsRepository(Context context) =>
        this.context = context;

    public async Task<Maybe<double>> WorkingDays(User user)
    {
        return (await context.QuerySingleOrDefault<UserSetting>(
                "SELECT key AS Key, value AS Value, type AS Type FROM user_settings WHERE key IS @Key AND user_id IS @UserId",
                new
                {
                    Key = nameof(UserSettings.WorkingDays),
                    UserId = user.Id
                }))
            .ToMaybe()
            .Map(setting => double.Parse(setting.Value));
    }
}