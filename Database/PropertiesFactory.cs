using System.Collections.Generic;
using System.Configuration;
using NDesk.Options;

namespace Database
{
    public class PropertiesFactory
    {
        public static Properties GetProperties(IEnumerable<string> args)
        {
            var properties = new Properties();
            var options = new OptionSet
            {
            {   "c|connectionstring=",
                "(MANDATORY) The connectionstring for the database to upgrade",
                v => properties.UpgradeConnectionString = v },
            {   "q|quiet=",
                "Run quietly - don't prompt for user input",
                q => properties.Quiet = q },
            {   "d|drop=",
                "Drop the database if it exists already",
                d => properties.Drop = d },
            {   "m|master=",
                "(MANDATORY) The connectionstring for the master database",
                m => properties.MasterConnectionString = m },
            {   "f|folder=",
                "(MANDATORY) The folder where database files will be created",
                f => properties.DatabaseFileLocation = f },
            {   "n|name=",
                "(MANDATORY) The name of the database to create or use",
                n => properties.DatabaseName = n },

            };
            options.Parse(args);

            properties.UpgradeConnectionString = properties.UpgradeConnectionString ?? GetConnectionString("Application");
            properties.MasterConnectionString = properties.MasterConnectionString ?? GetConnectionString("MASTER");
            properties.Drop = properties.Drop ?? GetAppSetting("DropDatabase");
            properties.DatabaseFileLocation = properties.DatabaseFileLocation ?? GetAppSetting("DatabaseFileLocation");
            properties.DatabaseName = properties.DatabaseName ?? GetAppSetting("DatabaseName");

            return properties;
        }

        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];

        }

        private static string GetConnectionString(string connectionStringName)
        {
            var cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            return cs != null ? cs.ConnectionString : null;
        }
    }
}
