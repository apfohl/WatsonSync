using System.Text.Json.Serialization;
using WatsonSync.Components;

namespace WatsonSync.Models;

public sealed record Frame
{
    public Frame(Guid id, DateTime startAt, DateTime endAt, string project, IEnumerable<string> tags)
    {
        Id = id;
        StartAt = startAt;
        EndAt = endAt;
        Project = project;
        Tags = tags;
    }

    public Frame() {}
    
    [JsonConverter(typeof(FrameGuidConverter))]
    public Guid Id { get; set; }

    [JsonPropertyName("start_at")] public DateTime StartAt { get; set; }

    [JsonPropertyName("end_at")] public DateTime EndAt { get; set; }
    public string Project { get; set; }
    public IEnumerable<string> Tags { get; set; }

    public void Deconstruct(out Guid id, out DateTime startAt, out DateTime endAt, out string project,
        out IEnumerable<string> tags)
    {
        id = Id;
        startAt = StartAt;
        endAt = EndAt;
        project = Project;
        tags = Tags;
    }
}