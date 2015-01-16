using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Repositories.NHibernate;
using NHibernate;
using NUnit.Framework;
using Domain.Interfaces;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class UserRepositoryFixture : ContainerFixture
    {
        private ISession _session;
        private IUserRepository _userRepository;
        private ICryptographyProvider _cryptographyProvider;

        [SetUp]
        public void SetUp()
        {
            _session = Container.Resolve<ISession>();
            _cryptographyProvider = new CryptographyProvider();
            _userRepository = new UserRepository(new SessionManager(() => _session), _cryptographyProvider);
        }

        [Test]
        public void Given_a_user_in_both_admin_and_user_roles_Roles_exist()
        {
            var user = _userRepository.GetByUsername("admin");

            Assert.IsNotNull(user, "User");
            Assert.IsTrue(user.Roles.Any(x => x.Name == "Administrators"));
            Assert.IsTrue(user.Roles.Any(x => x.Name == "Users"));
        }

        [Test]
        public void Given_a_role_name_roles_are_returned()
        {
            var users = _userRepository.GetUsersInRole("Users");
            
            Assert.IsTrue(users.Any(u => u.Username == "admin"));
            Assert.IsTrue(users.Any(u => u.Username == "user"));
        }
    }
}
