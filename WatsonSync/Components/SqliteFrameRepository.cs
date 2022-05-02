using WatsonSync.Models;

namespace WatsonSync.Components;

public class SqliteFrameRepository : IFrameRepository
{
    private readonly Context context;

    public SqliteFrameRepository(Context context) =>
        this.context = context;

    public async Task<IEnumerable<Frame>> QueryAll(User user) =>
        await context.Query<Frame>(
            "SELECT id AS Id, start_at AS StartAt, end_at AS EndAt, project AS Project FROM frames");

    public async Task<IEnumerable<Frame>> QuerySince(User user, DateTime since) =>
        await context.Query<Frame>(
            "SELECT id AS Id, start_at AS StartAt, end_at AS EndAt, project AS Project FROM frames WHERE end_at > @Since",
            new { Since = since });

    public async Task Insert(User user, IEnumerable<Frame> frames)
    {
        foreach (var frame in frames)
        {
            await context.Execute(
                "INSERT INTO frames (id, start_at, end_at, project) values (@Id, @StartAt, @EndAt, @Project)",
                frame);
        }
    }
}