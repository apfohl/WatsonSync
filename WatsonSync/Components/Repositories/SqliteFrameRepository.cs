using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public class SqliteFrameRepository : IFrameRepository
{
    private readonly Context context;

    public SqliteFrameRepository(Context context) =>
        this.context = context;

    public async Task<IEnumerable<Frame>> QueryAll(User user) =>
        await context.Query<Frame>(
            "SELECT id AS Id, begin_at AS BeginAt, end_at AS EndAt, project AS Project FROM frames WHERE user_id is @UserId",
            new { UserId = user.Id });

    public async Task<IEnumerable<Frame>> QuerySince(User user, DateTime since) =>
        await context.Query<Frame>(
            "SELECT id AS Id, begin_at AS BeginAt, end_at AS EndAt, project AS Project FROM frames WHERE user_id is @UserId AND end_at > @Since",
            new { UserId = user.Id, Since = since });

    public async Task Insert(User user, IEnumerable<Frame> frames)
    {
        foreach (var frame in frames)
        {
            await context.Execute(
                "INSERT INTO frames (id, begin_at, end_at, project, user_id) values (@Id, @BeginAt, @EndAt, @Project, @UserId)",
                new { frame.Id, frame.BeginAt, frame.EndAt, frame.Project, UserId = user.Id });
        }
    }
}