using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
	    User GetByLogin(string username, string password);

	    User GetByUsername(string username);

	    bool Exists(string username);

	    User GetByConfirmationToken(string confirmationToken);

	    IEnumerable<User> GetUsersInRole(string roleName);
	}
}
