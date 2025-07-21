using System.Data;
using Dapper;
using Domain.Primitives;
using Microsoft.Data.SqlClient;

namespace Persistence.TypeHandlers;

public class NameHandler : SqlMapper.TypeHandler<Name>
{
    public static readonly NameHandler Default = new();

    private NameHandler()
    {
    }

    public override void SetValue(IDbDataParameter parameter, Name value)
    {
        parameter.Value = value.ToString();

        if (parameter is SqlParameter sqlParameter)
            sqlParameter.SqlDbType = SqlDbType.Text;
    }

    public override Name Parse(object value)
    {
        return value is string name
            ? new Name(name)
            : throw new ArgumentException($"Cannot convert {value.GetType()} to {typeof(Name)}");
    }
}