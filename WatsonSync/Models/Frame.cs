using System.Text.Json.Serialization;
using WatsonSync.Components;

namespace WatsonSync.Models;

public sealed record Frame
{
    public Frame(Guid id, DateTime beginAt, DateTime endAt, string project, IEnumerable<string> tags)
    {
        Id = id;
        BeginAt = beginAt;
        EndAt = endAt;
        Project = project;
        Tags = tags;
    }

    public Frame()
    {
    }

    [JsonConverter(typeof(FrameGuidConverter))]
    public Guid Id { get; set; }

    [JsonPropertyName("begin_at")] public DateTime BeginAt { get; set; }

    [JsonPropertyName("end_at")] public DateTime EndAt { get; set; }
    public string Project { get; set; }
    public IEnumerable<string> Tags { get; set; }
}