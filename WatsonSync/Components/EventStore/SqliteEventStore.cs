using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;

namespace WatsonSync.Components.EventStore;

public sealed class SqliteEventStore : IEventStore
{
    private readonly Context context;

    private readonly Func<string, Guid, IEnumerable<Event>, Task> append =
        (bucket, aggregateId, events) => throw new NotImplementedException();

    private readonly Func<Task<IEnumerable<Event>>> read =
        () => throw new NotImplementedException();

    public SqliteEventStore(Context context) =>
        this.context = context;

    public IEventStream OpenStream(string bucket, Guid aggregateId) =>
        new EventStream(bucket, aggregateId, read, append.Apply(bucket, aggregateId));
}