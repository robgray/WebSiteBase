using Domain.Entities;
using Domain.QueryFilters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;

namespace UnitTests.Model.Filters
{
    [TestFixture]
    public class ExistingUserEmailAddressFilterTests
    {
        protected ExistingUserEmailAddressFilter filter;
    
        [SetUp]
        public void SetUp()
        {
            filter = new ExistingUserEmailAddressFilter();
        }

        [Test]
        public void Given_the_user_already_has_this_email_address_the_email_doesnt_already_exist()
        {
            var user = new List<User>
                {
                    new User {EmailAddress = "test@test.com"}.WithId(1)
                };

            filter.EmailAddress = "test@test.com";
            filter.UserId = 1;
            
            Assert.IsFalse(user.Any(filter.GetFilter().Compile()));
        }

        [Test]
        public void Given_another_user_already_has_this_email_address_the_email_already_exists()
        {
            var user = new List<User>
                {
                    new User {EmailAddress = "test@test.com"}.WithId(1)
                };

            filter.EmailAddress = "test@test.com";
            filter.UserId = 2;

            Assert.IsTrue(user.Any(filter.GetFilter().Compile()));
        }
    }
}
