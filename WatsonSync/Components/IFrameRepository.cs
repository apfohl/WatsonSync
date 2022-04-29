using WatsonSync.Models;

namespace WatsonSync.Components;

public interface IFrameRepository
{
    IEnumerable<Frame> QueryAll(User user);
    IEnumerable<Frame> QuerySince(User user, DateTime since);
    void Insert(User user, IEnumerable<Frame> frames);
}