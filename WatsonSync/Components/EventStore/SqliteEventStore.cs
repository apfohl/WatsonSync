using WatsonSync.Components.DataAccess;
using WatsonSync.Components.Extensions;

namespace WatsonSync.Components.EventStore;

public sealed class SqliteEventStore : IEventStore
{
    private readonly Context context;

    private Func<string, Guid, Task<IEnumerable<Event>>> Read =>
        (bucket, aggregateId) =>
            context.Query<Event>(
                "SELECT event_id AS Id, sequence_number AS SequenceNumber, timestamp AS Timestamp, payload AS Payload FROM event_streams WHERE bucket IS @Bucket AND aggregate_id IS @AggregateId ORDER BY sequence_number",
                new { Bucket = bucket, AggregateId = aggregateId }
            );

    private Func<string, Guid, IEnumerable<Event>, Task> Append =>
        (bucket, aggregateId, events) =>
            events.ForEach(async @event =>
            {
                await context.Execute(
                    "INSERT INTO event_streams (bucket, event_id, aggregate_id, sequence_number, event_name, timestamp, payload) VALUES (@Bucket, @EventId, @AggregateId, @SequenceNumber, @EventName, @Timestamp, @Payload)",
                    new
                    {
                        Bucket = bucket,
                        EventId = @event.Id,
                        AggregateId = aggregateId,
                        @event.SequenceNumber,
                        EventName = @event.Name,
                        @event.Timestamp,
                        @event.Payload
                    });
            });

    public SqliteEventStore(Context context) =>
        this.context = context;

    public IEventStream OpenStream(string bucket, Guid aggregateId) =>
        new EventStream(bucket, aggregateId, Read.Apply(bucket, aggregateId), Append.Apply(bucket, aggregateId));
}