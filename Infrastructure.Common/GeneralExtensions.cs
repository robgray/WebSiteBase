using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Common
{
    public static class GeneralExtensions
    {
        public static IDictionary<string,object> AnonymousObjectToDictionary(object obj)
        {
            return TypeDescriptor.GetProperties(obj)
                .OfType<PropertyDescriptor>()
                .ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(obj)
                );
        }

	    public static IDbCommand CreateCommand(this IDbConnection connection, string sql, object parameters = null)
	    {
		    var command = connection.CreateCommand();
		    command.CommandText = sql;

		    if (null != parameters)
				foreach (var pair in AnonymousObjectToDictionary(parameters))
				    command.Parameters.Add(new SqlParameter
					    {
						    ParameterName = pair.Key,
						    Value = pair.Value
					    });
		    return command;
	    }

		public static int ExecuteCommandNonQuery(this IDbConnection connection, string sql, object parameters = null)
	    {
		    return connection.CreateCommand(sql, parameters).ExecuteNonQuery();
	    }

		public static IDataReader ExecuteCommandReader(this IDbConnection connection, string sql, object parameters = null)
		{
			return connection.CreateCommand(sql, parameters).ExecuteReader();
		}

	    public static T ExecuteScalarCommand<T>(this IDbConnection connection, string sql, object parameters = null)
	    {
		    return (T) Convert.ChangeType(connection.CreateCommand(sql, parameters).ExecuteScalar(), typeof (T));
	    }
    }
}
