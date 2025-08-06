using BDD;
using Microsoft.Data.SqlClient;

namespace Integration;

public class TruncateDbSpecification : Specification
{
    protected static void Truncate(string connectionString)
    {
        const string the_command = """
                                   EXEC sp_MSForEachTable @command1='TRUNCATE TABLE ?'
                                   , @whereand = 'AND o.name <> ''SchemaVersions'''
                                   """;
        using var connection = new SqlConnection(connectionString);
        var command = connection.CreateCommand();
        command.CommandText = the_command;
        connection.Open();
        command.ExecuteNonQuery();
    }
}