using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using ApprovalTests;
using NUnit.Framework;

namespace UnitTests.Database.KeyTests
{
    [TestFixture]
    public class ForeignKeyTests
    {
        [Test]
        public void ForeignKeyMustBeCalledTargetId()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = foreignKeyNamesScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("{0}.{1}.{2} should be {3}{4}", reader[0], reader[1], reader[2], reader[3], reader[4]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        [Test]		
        public void ForeignKeyMustBeCalledFK_TableName_ColumnName()
        {
            var stringBuilder = new StringBuilder();
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = foreignKeyConstraintNamesScript;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(String.Format("Foreign key [{0}].[{1}].{2} should be named FK_{0}_{1}", reader[1], reader[2], reader[3]));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }

        private const string foreignKeyNamesScript = 
@"SELECT 
    KCU1.CONSTRAINT_SCHEMA,
    KCU1.TABLE_NAME AS 'FK_TABLE_NAME',
    KCU1.COLUMN_NAME AS 'FK_COLUMN_NAME',
    KCU2.TABLE_NAME AS 'UQ_TABLE_NAME',
    KCU2.COLUMN_NAME AS 'UQ_COLUMN_NAME'
FROM
    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1
    ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG 
        AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
        AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2
    ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG 
        AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA
        AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME
        AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION
   
WHERE
    KCU1.COLUMN_NAME != KCU2.TABLE_NAME + KCU2.COLUMN_NAME
ORDER BY
    KCU1.CONSTRAINT_SCHEMA,
    KCU1.TABLE_NAME,
    KCU1.COLUMN_NAME,
    KCU1.ORDINAL_POSITION";

        private const string foreignKeyConstraintNamesScript =
@"SELECT
    T.TABLE_SCHEMA
    ,T.TABLE_NAME
	,K.COLUMN_NAME  
    ,T.CONSTRAINT_NAME
FROM  
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS T 
    INNER JOIN 
    INFORMATION_SCHEMA.KEY_COLUMN_USAGE K 
    ON T.CONSTRAINT_NAME = K.CONSTRAINT_NAME  
WHERE 
    T.CONSTRAINT_TYPE = 'FOREIGN KEY'  
    AND T.CONSTRAINT_NAME != 'FK_' + T.TABLE_NAME + '_' + K.COLUMN_NAME
ORDER BY 
    T.TABLE_SCHEMA,    
    T.TABLE_NAME, 
    K.ORDINAL_POSITION";
    }


}
