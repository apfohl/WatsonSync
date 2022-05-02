using WatsonSync.Models;

namespace WatsonSync.Components;

public interface IContextFactory
{
    Context Create();
}