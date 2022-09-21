namespace WatsonSync.Components.EventStore;

public sealed record Event(Guid Id, int SequenceNumber, string Name, DateTime Timestamp, string Payload);
