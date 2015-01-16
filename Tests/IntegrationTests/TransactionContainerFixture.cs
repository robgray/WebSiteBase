using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NUnit.Framework;

namespace IntegrationTests
{
    public class TransactionContainerFixture : ContainerFixture
    {
        protected TransactionScope TransactionScope;

        [SetUp]
        public void BaseSetUp()
        {
            TransactionScope = new TransactionScope();
        }

        [TearDown]
        public void BaseTearDown()
        {
            if (null != TransactionScope)
            {
                try
                {
                    TransactionScope.Dispose();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee);
                }
                TransactionScope = null;
            }
        }
    }
}
