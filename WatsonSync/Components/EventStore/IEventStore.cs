namespace WatsonSync.Components.EventStore;

public interface IEventStore
{
    IEventStream OpenStream(string bucket, Guid aggregateId);
}
