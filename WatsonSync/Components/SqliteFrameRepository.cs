using WatsonSync.Models;

namespace WatsonSync.Components;

public class SqliteFrameRepository : IFrameRepository
{
    public IEnumerable<Frame> QueryAll(User user)
    {
        return Enumerable.Empty<Frame>();
    }

    public IEnumerable<Frame> QuerySince(User user, DateTime since)
    {
        return Enumerable.Empty<Frame>();
    }

    public void Insert(User user, IEnumerable<Frame> frames)
    {
    }
}