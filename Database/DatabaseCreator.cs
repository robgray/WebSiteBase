using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public static class DatabaseCreator
    {
        public static void Create(Properties properties)
        {
            Console.Write("Creation requested. Creating... ");

            var sqlFormats = new[] {@"
                CREATE DATABASE [{0}] ON  PRIMARY 
                
                ( NAME = N'{0}_System', FILENAME = N'{1}\{0}_System.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 5120KB ) 

                 LOG ON 
                ( NAME = N'{0}_Log', FILENAME = N'{1}\{0}_Log.ldf' , SIZE = 65536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 5120KB)

                ",

                @"ALTER DATABASE [{0}] SET COMPATIBILITY_LEVEL = 100",

                @"
                IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
                BEGIN
                    EXEC [{0}].[dbo].[sp_fulltext_database] @action = 'enable'
                END
                ",

                @"ALTER DATABASE [{0}] SET ANSI_NULL_DEFAULT OFF",

                @"ALTER DATABASE [{0}] SET ANSI_NULLS OFF ",

                @"ALTER DATABASE [{0}] SET ANSI_PADDING OFF ",

                @"ALTER DATABASE [{0}] SET ANSI_WARNINGS OFF ",

                @"ALTER DATABASE [{0}] SET ARITHABORT OFF ",

                @"ALTER DATABASE [{0}] SET AUTO_CLOSE OFF ",

                @"ALTER DATABASE [{0}] SET AUTO_CREATE_STATISTICS ON ",

                @"ALTER DATABASE [{0}] SET AUTO_SHRINK OFF ",

                @"ALTER DATABASE [{0}] SET AUTO_UPDATE_STATISTICS ON ",

                @"ALTER DATABASE [{0}] SET CURSOR_CLOSE_ON_COMMIT OFF ",

                @"ALTER DATABASE [{0}] SET CURSOR_DEFAULT  GLOBAL ",

                @"ALTER DATABASE [{0}] SET CONCAT_NULL_YIELDS_NULL OFF ",

                @"ALTER DATABASE [{0}] SET NUMERIC_ROUNDABORT OFF ",

                @"ALTER DATABASE [{0}] SET QUOTED_IDENTIFIER OFF ",

                @"ALTER DATABASE [{0}] SET RECURSIVE_TRIGGERS OFF ",

                @"ALTER DATABASE [{0}] SET  DISABLE_BROKER ",

                @"ALTER DATABASE [{0}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF ",

                @"ALTER DATABASE [{0}] SET DATE_CORRELATION_OPTIMIZATION OFF ",

                @"ALTER DATABASE [{0}] SET TRUSTWORTHY OFF ",

                @"ALTER DATABASE [{0}] SET ALLOW_SNAPSHOT_ISOLATION OFF ",

                @"ALTER DATABASE [{0}] SET PARAMETERIZATION SIMPLE ",

                @"ALTER DATABASE [{0}] SET READ_COMMITTED_SNAPSHOT OFF ",

                @"ALTER DATABASE [{0}] SET HONOR_BROKER_PRIORITY OFF ",

                @"ALTER DATABASE [{0}] SET  READ_WRITE ",

                @"ALTER DATABASE [{0}] SET RECOVERY FULL ",

                @"ALTER DATABASE [{0}] SET  MULTI_USER ",

                @"ALTER DATABASE [{0}] SET PAGE_VERIFY CHECKSUM  ",

                @"ALTER DATABASE [{0}] SET DB_CHAINING OFF"                
                  
            };

            using (var dbc = new SqlConnection(properties.MasterConnectionString))
            {
                dbc.Open();
                Execute(properties, dbc, sqlFormats);
            }

            Console.WriteLine("Completed!");
        }

        private static void Execute(Properties properties, SqlConnection connection, IEnumerable<string> sqlFormats)
        {
            foreach (var format in sqlFormats)
            {
                var sql = string.Format(format, properties.DatabaseName, properties.DatabaseFileLocation);
                var cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
