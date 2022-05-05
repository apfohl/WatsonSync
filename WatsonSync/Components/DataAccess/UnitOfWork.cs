using WatsonSync.Components.Repositories;

namespace WatsonSync.Components.DataAccess;

public sealed class UnitOfWork : IDisposable
{
    private readonly Context context;
    private readonly Lazy<IUserRepository> userRepository;
    private readonly Lazy<IFrameRepository> frameRepository;

    public IUserRepository Users =>
        userRepository.Value;

    public IFrameRepository Frames =>
        frameRepository.Value;

    public UnitOfWork(IContextFactory contextFactory)
    {
        context = contextFactory.Create();
        userRepository = new Lazy<IUserRepository>(() => new SqliteUserRepository(context));
        frameRepository = new Lazy<IFrameRepository>(() => new SqliteFrameRepository(context));
    }

    public Task Save() =>
        context.Commit();

    public void Dispose() =>
        context.Dispose();
}