using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public interface IUserSettingsRepository
{
    Task<Maybe<double>> WorkingDays(User user);
}