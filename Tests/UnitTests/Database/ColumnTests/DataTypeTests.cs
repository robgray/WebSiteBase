using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace UnitTests.Database.ColumnTests
{
    [TestFixture]       
    public class DataTypeTests
    {
        [Test]
        //[Ignore] // Ignore until we've updated existing VARCHAR to NVARCHAR
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void EnsureNVARCHARIsUsedAndNotVARCHAR()
        {
            ApprovalsExtensions.VerifyExecutedSql(VarcharCheckScript);
        }
        
        private const string VarcharCheckScript =
            @"SELECT TABLE_SCHEMA + '.' + TABLE_NAME + '(' + COLUMN_NAME + ')'
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE DATA_TYPE='varchar'";
    }
}
