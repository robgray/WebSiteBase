using System;
using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace Database
{
    public static class DatabaseUpdater
    {
        public static DatabaseUpgradeResult RunDatabaseUpgrade(string connectionString, bool logToConsole = false, string dropAndRecreateArgs = null)
        {
            if (dropAndRecreateArgs != null)
            {
                var args = dropAndRecreateArgs.Split(new [] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var properties = PropertiesFactory.GetProperties(args);
                if (!properties.IsValid())
                {
                    return new DatabaseUpgradeResult(null, false, new InvalidOperationException("Bad drop and recreate args"));                    
                }

                if (properties.IsDrop)
                {
                    Console.WriteLine("Drop database specified.");
                    if (EntryPoint.DoesDatabaseAlreadyExist(properties))
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

                if (EntryPoint.DoesDatabaseAlreadyExist(properties))
                {
                    Console.WriteLine("Database already exists. No creation needed");
                }
                else
                {
                    DatabaseCreator.Create(properties);
                }

            }

            var scriptsAssembly = Assembly.GetExecutingAssembly();
            
            var builder = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(scriptsAssembly)
                .LogToTrace();
            
            if (logToConsole)
                builder.LogToConsole();

            builder.Configure(c => c.ScriptExecutor.ExecutionTimeoutSeconds = 300);
            var result = builder.Build().PerformUpgrade();
            
            return result;
        }        
    }   
}