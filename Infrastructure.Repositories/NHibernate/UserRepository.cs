using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Interfaces;
using NHibernate.Linq;

namespace Infrastructure.Repositories.NHibernate
{
    public class UserRepository : AbstractRepository<User>, IUserRepository
    {
        private readonly ICryptographyProvider _cryptographyProvider;

	    public UserRepository(ISessionManager sessionManager, ICryptographyProvider cryptographyProvider)
	        : base(sessionManager)
	    {
	        _cryptographyProvider = cryptographyProvider;
	    }

        public User GetByLogin(string username, string password)
        {
            var user = Query.SingleOrDefault(u => u.Username == username);

            if (user == null || !user.IsPassword(_cryptographyProvider, password))
                return null;

            return user;
        }

        public User GetByUsername(string username)
        {
            var user = Query.SingleOrDefault(u => u.Username == username);

            return user;
        }

        public bool Exists(string username)
        {
            return Query.Any(x => x.Username == username);
        }

        public User GetByConfirmationToken(string confirmationToken)
        {
            return Query.SingleOrDefault(u => u.ConfirmationToken == confirmationToken);
        }

        public IEnumerable<User> GetUsersInRole(string roleName)
        {
            var role = Session.Query<Role>().SingleOrDefault(r => r.Name == roleName);
            return role == null ? new List<User>() : role.Users;
        }
    }
}
