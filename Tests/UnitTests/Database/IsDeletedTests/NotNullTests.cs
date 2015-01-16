using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace UnitTests.Database.IsDeletedTests
{
    [TestFixture]
    public class NotNullTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void IsDeletedIsNotNullable()
        {
            ApprovalsExtensions.VerifyExecutedSql(defaultSql);
        }

        private const string defaultSql = @"

SELECT 
    TABLE_SCHEMA + '.' + TABLE_NAME
FROM
    INFORMATION_SCHEMA.COLUMNS
WHERE
    COLUMN_NAME = 'IsDeleted'
    AND IS_NULLABLE = 'YES'";

    }
}