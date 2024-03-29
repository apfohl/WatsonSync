namespace WatsonSync.Components.DataAccess;

public interface IContextFactory
{
    Context Create();
    ReadOnlyContext CreateReadOnly();
}