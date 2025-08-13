using System.Text.Json.Serialization;
using Events.Domain.Primitives;

namespace Events.Domain;

public static class Converters
{
    public static List<JsonConverter> GetConverters =>
    [
        new EventNameConverter(),
    ];
}