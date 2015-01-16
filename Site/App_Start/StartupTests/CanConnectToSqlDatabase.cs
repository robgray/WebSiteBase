using System;
using System.Configuration;
using System.Data.SqlClient;
using Castle.Core.Logging;

namespace WebBase.Mvc.App_Start.StartupTests
{
	public class CanConnectToSqlDatabase : IStartupTest
	{
		public ILogger Logger { get; set; }

		public void RunTest()
		{
			// This is a very naive test that just checks we can connect to the database
			var connectionStringSettings = ConfigurationManager.ConnectionStrings["Application"];
			if (connectionStringSettings == null)
				throw new Exception("The connection string called Application is not specified in the configuration file.");

			var connectionString = connectionStringSettings.ConnectionString;
			Logger.InfoFormat("Attempting to connect to sql database at '{0}'", connectionString);
			using (var conn = new SqlConnection(connectionString)) {
				conn.Open();
			}
			Logger.InfoFormat("Connecting to sql database at '{0}' - PASSED!", connectionString);
		}
	}
}