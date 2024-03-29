using WatsonSync.Models;

namespace WatsonSync.Components.Repositories;

public interface IFrameRepository
{
    Task<IEnumerable<Frame>> QuerySince(User user, DateTime since);
    Task InsertOrReplace(User user, IEnumerable<Frame> frames);
}