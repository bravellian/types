using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Bravillian;

[JsonSourceGenerationOptions(System.Text.Json.JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(JsonNode))]
internal partial class SourceGenerationContext : JsonSerializerContext;