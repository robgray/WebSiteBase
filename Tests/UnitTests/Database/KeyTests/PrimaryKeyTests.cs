using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using ApprovalTests;
using NUnit.Framework;

namespace UnitTests.Database.KeyTests
{
    [TestFixture]
    public class PrimaryKeyTests
    {
        [Test]
        public void PrimaryKeyMustBeCalledId()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = primaryKeysScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("{0}.{1}.{2}", reader[0], reader[1], reader[2]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        [Test]		
        public void PrimaryKeyMustBeCalledPK_TableName()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = primaryKeyNameScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("The primary key constraint {1} should be named PK_{0}", reader[1], reader[2]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        [Test]
		[Ignore]
        public void PrimaryKeyMustBeLocatedOnTheFileGroupMatchingItsSchema()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = primaryKeyFilegroupScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("The db object [{0}].[{1}].{2} should be located on the [{0}] filegroup", reader[0], reader[1], reader[2] ?? "No name"));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        private const string primaryKeysScript = @"
SELECT  
    T.TABLE_SCHEMA
    ,T.TABLE_NAME  
    ,K.COLUMN_NAME  
FROM  
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS T 
    INNER JOIN 
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE K 
    ON T.CONSTRAINT_NAME = K.CONSTRAINT_NAME  
WHERE 
    T.CONSTRAINT_TYPE = 'PRIMARY KEY'  
    AND K.COLUMN_NAME != 'Id'
ORDER BY 
    T.TABLE_SCHEMA,    
    T.TABLE_NAME, 
    K.ORDINAL_POSITION";



        private const string primaryKeyNameScript = @"
SELECT  
    T.TABLE_SCHEMA
    ,T.TABLE_NAME  
    ,T.CONSTRAINT_NAME  
FROM  
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS T 
    INNER JOIN 
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE K 
    ON T.CONSTRAINT_NAME = K.CONSTRAINT_NAME  
WHERE 
    T.CONSTRAINT_TYPE = 'PRIMARY KEY'  
    AND T.CONSTRAINT_NAME != 'PK_' + T.TABLE_NAME
ORDER BY 
    T.TABLE_SCHEMA,    
    T.TABLE_NAME, 
    K.ORDINAL_POSITION";



        private const string primaryKeyFilegroupScript =
@"SELECT 
	SCHEMA_NAME(schema_id) AS [Schema],
	o.[name] AS [Table],
	i.[name] AS [ObjName],
	f.[name] AS [Filegroup]
FROM 
	sys.indexes i
	INNER JOIN sys.filegroups f
		ON i.data_space_id = f.data_space_id
	INNER JOIN sys.all_objects o
		ON i.[object_id] = o.[object_id]
WHERE
	i.data_space_id = f.data_space_id
	AND o.type = 'U'
	AND SCHEMA_NAME(schema_id) != f.[name]";


    }
}
