using WatsonSync.Components.DataAccess;
using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public sealed class SqliteFrameRepository : IFrameRepository
{
    private readonly Context context;

    public SqliteFrameRepository(Context context) =>
        this.context = context;

    public Task<IEnumerable<Frame>> QueryAll(User user) =>
        QuerySince(user, DateTime.MinValue);

    public async Task<IEnumerable<Frame>> QuerySince(User user, DateTime since) =>
        await context.Query(
            "SELECT id AS Id, begin_at AS BeginAt, end_at AS EndAt, project AS Project, tags AS Tags FROM frames WHERE user_id IS @UserId AND end_at > @Since",
            new[]
            {
                typeof(Frame),
                typeof(string)
            },
            objects =>
            {
                var frame = (Frame)objects[0];
                var tags = (string)objects[1];
                frame.Tags = tags.Split(',');
                return frame;
            },
            "tags",
            new { UserId = user.Id, Since = since });

    public async Task Insert(User user, IEnumerable<Frame> frames)
    {
        foreach (var frame in frames)
        {
            var tags = string.Join(',', frame.Tags);

            await context.Execute(
                "INSERT INTO frames (id, begin_at, end_at, project, user_id, tags) VALUES (@Id, @BeginAt, @EndAt, @Project, @UserId, @Tags)",
                new { frame.Id, frame.BeginAt, frame.EndAt, frame.Project, UserId = user.Id, Tags = tags });
        }
    }
}