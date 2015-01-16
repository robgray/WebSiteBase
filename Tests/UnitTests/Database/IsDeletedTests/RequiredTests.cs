using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using ApprovalTests;
using NUnit.Framework;

namespace UnitTests.Database.IsDeletedTests
{
    [TestFixture]
    public class RequiredTests
    {
        [Test]
        public void TablesMustHaveIsDeleted()
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = defaultSql;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stringBuilder.AppendLine(reader.GetString(0));
                    }
                }
            }

            Approvals.Verify(stringBuilder);
        }
        
        private const string defaultSql = @"

SELECT 
    DISTINCT table_schema +  '.' +  table_name AS TableName
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    (table_schema <> 'dbo')
    AND table_schema +  '.' +  table_name NOT IN (
        SELECT
            table_schema +  '.' +  table_name FROM INFORMATION_SCHEMA.COLUMNS
        WHERE
            COLUMN_NAME = 'IsDeleted'
            AND table_schema <> 'dbo')
ORDER BY
    TableName";

    }
}
