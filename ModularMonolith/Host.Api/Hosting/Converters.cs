using System.Text.Json.Serialization;

namespace Api.Hosting;

public static class Converters
{
    public static List<JsonConverter> GetConverters =>
    [
        new JsonStringEnumConverter()
    ];
}