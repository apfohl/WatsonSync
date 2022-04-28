using WatsonSync.Models;

namespace WatsonSync.Components;

public interface IFrameRepository
{
    IEnumerable<Frame> QueryAll();
    IEnumerable<Frame> QuerySince(DateTime since);
}