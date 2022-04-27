using System.Text.Json.Serialization;
using WatsonSync.Components;

namespace WatsonSync.Models;

public sealed record Frame(
    [property:JsonConverter(typeof(FrameGuidConverter))]
    Guid Id, 
    [property:JsonPropertyName("start_at")]
    DateTime StartAt, 
    [property:JsonPropertyName("end_at")]
    DateTime EndAt, 
    string Project, 
    IEnumerable<string> Tags);