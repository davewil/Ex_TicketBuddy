using System.Reflection;
using DbUp;

namespace Tickets.Migrations;

public static class Migration
{
    public static bool Upgrade(string connectionString)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            return false;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return true;
    }
}