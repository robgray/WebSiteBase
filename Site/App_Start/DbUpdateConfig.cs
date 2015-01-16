using System;
using Database;
using DbUp.Engine;
using Infrastructure.Common;

namespace WebBase.Mvc.App_Start
{
	public class DbUpdateConfig
	{
        public static DbUpStatus UpgradeDatabase(string dropAndRecreateArgs = null)
		{
			var dbUpStatus = new DbUpStatus { Success = true };
			try {
				var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Application"]
					.ConnectionString;

				var dbupConnectionString = connectionString.ToLower();

                dbUpStatus.Result = DatabaseUpdater.RunDatabaseUpgrade(dbupConnectionString, dropAndRecreateArgs: dropAndRecreateArgs);
			}
			catch (Exception ex) {
				dbUpStatus.Success = false;
				dbUpStatus.Error = ex;
			}

			dbUpStatus.LastUpdated = DateTimeHelper.Now;

			return dbUpStatus;
		}
	}

	public class DbUpStatus
	{
		public bool Success { get; set; }
		public Exception Error { get; set; }

		public DatabaseUpgradeResult Result { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}