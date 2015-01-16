using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace UnitTests.Database.IsDeletedTests
{
    [TestFixture]
    public class DefaultTests
    {
        [Test]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void IsDeletedHasDefaultOfFalse()
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
    AND ((COLUMN_DEFAULT <> '((0))' AND COLUMN_DEFAULT <> '(0)') OR COLUMN_DEFAULT IS NULL)
ORDER BY
    1";

    }
}