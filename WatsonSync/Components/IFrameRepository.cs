using WatsonSync.Models;

namespace WatsonSync.Components;

public interface IFrameRepository
{
    Task<IEnumerable<Frame>> QueryAll(User user);
    Task<IEnumerable<Frame>> QuerySince(User user, DateTime since);
    Task Insert(User user, IEnumerable<Frame> frames);
}