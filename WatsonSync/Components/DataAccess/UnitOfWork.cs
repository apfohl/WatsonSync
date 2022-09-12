using WatsonSync.Components.Repositories;

namespace WatsonSync.Components.DataAccess;

public sealed class UnitOfWork : IDisposable, IAsyncDisposable
{
    private readonly Context context;
    private readonly Lazy<IFrameRepository> frameRepository;
    private readonly Lazy<IUserRepository> userRepository;
    private readonly Lazy<IUserSettingsRepository> userSettingsRepository;

    public UnitOfWork(IContextFactory contextFactory)
    {
        context = contextFactory.Create();
        userRepository = new Lazy<IUserRepository>(() => new SqliteUserRepository(context));
        frameRepository = new Lazy<IFrameRepository>(() => new SqliteFrameRepository(context));
        userSettingsRepository = new Lazy<IUserSettingsRepository>(() => new SqliteUserSettingsRepository(context));
    }

    public IUserRepository Users =>
        userRepository.Value;

    public IFrameRepository Frames =>
        frameRepository.Value;

    public IUserSettingsRepository UserSettings =>
        userSettingsRepository.Value;

    public void Dispose() =>
        context.Dispose();

    public Task Save() =>
        context.Commit();

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}