using System.Text.Json.Serialization;
using Domain.Primitives;

namespace Domain;

public static class Converters
{
    public static List<JsonConverter> GetConverters =>
    [
        new EventNameConverter(),
    ];
}