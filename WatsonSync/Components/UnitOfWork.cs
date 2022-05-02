using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components;

public sealed class UnitOfWork : IDisposable
{
    private readonly Context context;
    private Maybe<IUserRepository> userRepository;
    private Maybe<IFrameRepository> frameRepository;

    public IUserRepository UserRepository =>
        userRepository.Match(
            repository => repository,
            () =>
            {
                var repository = new SqliteUserRepository(context);
                userRepository = repository.Just<IUserRepository>();
                return repository;
            });

    public IFrameRepository FrameRepository =>
        frameRepository.Match(
            repository => repository,
            () =>
            {
                var repository = new SqliteFrameRepository(context);
                frameRepository = repository.Just<IFrameRepository>();
                return repository;
            });

    public UnitOfWork(IContextFactory contextFactory) =>
        context = contextFactory.Create();

    public Task Save() =>
        context.Commit();

    public void Dispose() =>
        context.Dispose();
}