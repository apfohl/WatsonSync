namespace WatsonSync.Components.EventStore;

public sealed class EventStream : IEventStream
{
    public string Bucket { get; }
    public Guid AggregateId { get; }
    private readonly Func<Task<IEnumerable<Event>>> read;
    private readonly Func<IEnumerable<Event>, Task> append;

    public EventStream(
        string bucket,
        Guid aggregateId,
        Func<Task<IEnumerable<Event>>> read,
        Func<IEnumerable<Event>, Task> append)
    {
        this.read = read;
        this.append = append;
        Bucket = bucket;
        AggregateId = aggregateId;
    }

    public Task<IEnumerable<Event>> Read() =>
        read();

    public Task Append(IEnumerable<Event> events) =>
        append(events);
}