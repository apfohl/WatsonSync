using System.Text.Json;
using System.Text.Json.Serialization;

namespace WatsonSync.Components;

public sealed class FrameGuidConverter : JsonConverter<Guid>
{
    private const string Prefix = "urn:uuid:";

    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Guid.Parse(reader.GetString()!.Remove(0, Prefix.Length));

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options) =>
        writer.WriteStringValue($"{Prefix}{value.ToString()}");
}