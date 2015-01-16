using System.Data.SqlClient;

namespace Database
{
    public static class DatabaseDropper
    {
        public static void Drop(Properties properties)
        {
            const string sqlFormat = @"

                IF  EXISTS (SELECT name FROM [master].[sys].databases WHERE name = N'{0}')
                BEGIN
	                ALTER DATABASE [{0}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
	                DROP DATABASE [{0}]
                END
                        ";

            var sql = string.Format(sqlFormat, properties.DatabaseName);

            using (var dbc = new SqlConnection(properties.MasterConnectionString))
            {
                using (var cmd = new SqlCommand(sql, dbc))
                {
                    dbc.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}