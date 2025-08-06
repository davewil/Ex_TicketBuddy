using Testcontainers.MsSql;

namespace Testing.Integration;

public static class MsSqlContainerExtensions
{
    public static string GetTicketBuddyConnectionString(this MsSqlContainer container)
    {
        return container.GetConnectionString().Replace("master", "TicketBuddy.Tickets");
    }
}