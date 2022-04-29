using WatsonSync.Models;

namespace WatsonSync.Components;

public class SqliteFrameRepository : IFrameRepository
{
    private readonly string databasePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "watson-sync.sqlite");

    public async Task<IEnumerable<Frame>> QueryAll(User user)
    {
        using var unitOfWork = await UnitOfWork.Create($"Data Source={databasePath}; Pooling=false");

        var frames = await unitOfWork.Query<Frame>("SELECT id AS Id, start_at AS StartAt, end_at AS EndAt, project AS Project FROM frames");

        await unitOfWork.Commit();

        return frames;
    }

    public async Task<IEnumerable<Frame>> QuerySince(User user, DateTime since)
    {
        using var unitOfWork = await UnitOfWork.Create($"Data Source={databasePath}; Pooling=false");

        var frames = await unitOfWork.Query<Frame>("SELECT id AS Id, start_at AS StartAt, end_at AS EndAt, project AS Project FROM frames WHERE end_at > @Since", new { Since = since });

        await unitOfWork.Commit();

        return frames;
    }

    public async Task Insert(User user, IEnumerable<Frame> frames)
    {
        using var unitOfWork = await UnitOfWork.Create($"Data Source={databasePath}; Pooling=false");

        foreach (var frame in frames)
        {
            await unitOfWork.Execute(
                "INSERT INTO frames (id, start_at, end_at, project) values (@Id, @StartAt, @EndAt, @Project)",
                frame);
        }

        await unitOfWork.Commit();
    }
}