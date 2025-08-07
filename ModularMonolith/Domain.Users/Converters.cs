using System.Text.Json.Serialization;
using Domain.Users.Primitives;

namespace Domain.Users;

public static class UsersConverters
{
    public static List<JsonConverter> GetConverters =>
    [
        new NameConverter(),
        new EmailConverter(),
    ];
}