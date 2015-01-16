using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace UnitTests.Database.IndexTests
{
    [TestFixture]
    public class ClusteredIndexTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void AllTablesMustHaveAClusteredIndex()
        {
            ApprovalsExtensions.VerifyExecutedSql(defaultSql);
        }

        private const string defaultSql = @"

SELECT
    sc.name + '.' + so.name  
FROM sys.objects so
    INNER JOIN sys.schemas sc ON so.schema_id = sc.schema_id
    LEFT JOIN sys.indexes si ON si.[object_id] = so.[object_id] AND si.type_desc = 'CLUSTERED'
WHERE 
    so.type = 'U' 
    AND si.object_id IS null 
    AND so.name != 'SchemaVersions'
ORDER BY sc.name, so.name";

    }
}