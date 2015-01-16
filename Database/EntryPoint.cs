using System;
using System.Data.SqlClient;

namespace Database
{
    public class EntryPoint
    {
        static void Main(string[] args)
        {
            var started = DateTime.Now;
            Console.WriteLine("Deploy time started: {0}", started.ToShortTimeString());
            var properties = PropertiesFactory.GetProperties(args);
            if (!properties.IsValid())
            {
                Console.WriteLine("Missing required properties. Please supply them via the command line or the app.config file.");
                return;
            }

            if (properties.IsDrop)
            {
                Console.WriteLine("Drop database specified.");
                if (DoesDatabaseAlreadyExist(properties))
                {
                    Console.Write("Database exists. Dropping...");
                    DatabaseDropper.Drop(properties);
                    Console.WriteLine("Complete!");
                }
                else
                {
                    Console.WriteLine("Database does not exist, nothing to drop.");
                }
            }
            
            if (DoesDatabaseAlreadyExist(properties))
            {
                Console.WriteLine("Database already exists. No creation needed");
            }
            else
            {
                DatabaseCreator.Create(properties);
            }

            Console.WriteLine("Upgrading database...");
            var result = DatabaseUpdater.RunDatabaseUpgrade(properties.UpgradeConnectionString, true);
            if (result.Successful == false)
            {
                Console.WriteLine("Upgrade failed.");
                throw result.Error;
            }
            Console.WriteLine("Database upgrade completed!");

            var finished = DateTime.Now;
            var timeTakenMinutes = finished.Subtract(started).TotalMinutes;
            Console.WriteLine("Deploy finish time: {0}", finished.ToShortTimeString());
            Console.WriteLine("Time taken: {0} minutes", timeTakenMinutes);

            if (!properties.IsQuiet)
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        public static bool DoesDatabaseAlreadyExist(Properties properties)
        {
            using (var dbc = new SqlConnection(properties.MasterConnectionString))
            {
                dbc.Open();
                var sql = string.Format("SELECT COUNT(*) FROM sys.databases WHERE name = N'{0}'", properties.DatabaseName);
                using (var cmd = new SqlCommand(sql, dbc))
                {
                    var count = (int)cmd.ExecuteScalar();
                    return count >= 1;
                }
            }
        }
    }    
}
