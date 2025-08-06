using System.Text.Json.Serialization;
using Domain.Primitives;
using Users.Domain.Primitives;

namespace Domain;

public static class Converters
{
    public static List<JsonConverter> GetConverters =>
    [
        new EventNameConverter(),
        new NameConverter(),
        new EmailConverter(),
    ];
}