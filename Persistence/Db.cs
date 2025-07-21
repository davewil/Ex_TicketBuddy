using System.Data;
using Dapper;
using Domain;
using Microsoft.Data.SqlClient;
using Persistence.TypeHandlers;

namespace Persistence
{
    public class Db
    {
        static Db()
        {
            SqlMapper.AddTypeHandler(NameHandler.Default);
        }

        private string ConnectionString { get; }

        public Db(string connectionString)
        {
            Validation.BasedOn(errors =>
            {
                if(string.IsNullOrEmpty(connectionString))
                  errors.Add("A connection string is required.");
            });

            ConnectionString = connectionString;
        }

        private static SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public async Task ExecuteAsync(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            await using var connection = CreateConnection(ConnectionString);
            await connection.ExecuteAsync(sql, param, commandType: commandType).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CommandType commandType = CommandType.Text)
        {
            await using var connection = CreateConnection(ConnectionString);
            return await connection.QueryAsync<T>(sql, param, commandType: commandType).ConfigureAwait(false);
        }
    }
}