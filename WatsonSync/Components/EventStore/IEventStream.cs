namespace WatsonSync.Components.EventStore;

public interface IEventStream
{
    string Bucket { get; }
    Guid AggregateId { get; }
    Task<IEnumerable<Event>> Read();
    Task Append(IEnumerable<Event> events);
}
