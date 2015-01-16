using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using ApprovalTests;

namespace UnitTests.Database
{
    public static class ApprovalsExtensions
    {
        public static void VerifyExecutedSql(string sql)
        {
            var stringBuilder = new StringBuilder();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FizzioFit"].ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
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
    }
}