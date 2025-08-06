using Testcontainers.MsSql;

namespace Integration;

public static class MsSqlContainerExtensions
{
    public static string GetTicketBuddyConnectionString(this MsSqlContainer container)
    {
        return container.GetConnectionString().Replace("master", "TicketBuddy.Users");
    }
}