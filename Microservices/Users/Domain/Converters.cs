using System.Text.Json.Serialization;
using Users.Domain.Primitives;

namespace Users.Domain;

public static class Converters
{
    public static List<JsonConverter> GetConverters =>
    [
        new NameConverter(),
        new EmailConverter(),
    ];
}