using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using ApprovalTests;
using NUnit.Framework;

namespace UnitTests.Database.ConstraintTests
{
    [TestFixture] 
    public class ConstraintTests
    {
        [Test]
        public void DefaultConstraintMustBeCalledDEF_TableName_ColumnName()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = defaultConstraintNamesScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("Default constraint {0}.{1}.{2} should be named DEF_{0}_{1}", reader[0], reader[1], reader[2]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        [Test]
        public void CheckConstraintNameMustStartWithCHK_SchemaName_TableName_()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = checkConstraintNamesScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("Check constraint [{0}].[{1}].{2} should have a name that starts with CHK_{0}_{1}_", reader[0], reader[1], reader[2]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        [Test]
        public void UniqueConstraintMustBeCalledUQ_TableName_ColumnName()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = uniqueConstraintNamesScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("Unique constraint [{0}].{2} should be called UQ_{0}_{1}", reader[1], reader[2], reader[3]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        private const string defaultConstraintNamesScript =
@"SELECT 	
	T.name AS TABLE_NAME,
	A.name AS COLUMN_NAME,
	C.name AS CONSTRAINT_NAME
FROM  
	SYS.all_columns A
	INNER JOIN 
	SYS.TABLES T  
	ON A.object_id = T.object_id 
	inner join 
	SYS.default_constraints C 
	on A.default_object_id = C.object_id 
WHERE 
	C.type_desc = 'DEFAULT_CONSTRAINT'
	AND C.name != 'DEF_' + T.name + '_' + A.name 
ORDER BY 
    T.name,    
    A.name";

        private const string checkConstraintNamesScript =
@"SELECT 
	SCHEMA_NAME(schema_id) AS SchemaName,
	OBJECT_NAME(parent_object_id) AS TableName,
	OBJECT_NAME(OBJECT_ID) AS NameofConstraint,	
	'CHK_' + SCHEMA_NAME(schema_id) + '_' + OBJECT_NAME(parent_object_id) + '_' AS StartsWith
FROM
	sys.objects
WHERE
	type_desc LIKE 'CHECK_CONSTRAINT'
	AND OBJECT_NAME(OBJECT_ID) NOT LIKE 'CHK_' + SCHEMA_NAME(schema_id) + '_' + OBJECT_NAME(parent_object_id) + '_%'
ORDER BY
    SchemaName,
    TableName";

        private const string uniqueConstraintNamesScript = 
@"SELECT 
    T.TABLE_SCHEMA
    ,T.TABLE_NAME
	,K.COLUMN_NAME  
    ,T.CONSTRAINT_NAME
FROM  
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS T 
    INNER JOIN 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE K 
    ON T.CONSTRAINT_NAME = K.CONSTRAINT_NAME
WHERE
	CONSTRAINT_TYPE = 'UNIQUE'
	AND T.CONSTRAINT_NAME != 'UQ_' + T.TABLE_NAME + '_' + K.COLUMN_NAME
ORDER BY 
    T.TABLE_SCHEMA,    
    T.TABLE_NAME";

    }
}