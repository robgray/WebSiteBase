using System.Security.Principal;

namespace WebBase.Mvc.Plumbing
{
	public interface IWebSecurity
	{
		bool Login(string userName, string password, bool persistCookie = false);
		void Logout();
		string CreateUserAndAccount(string userName, string password, object propertyValues = null,
			   bool requireConfirmationToken = false);
		int GetUserId(string userName);
		bool ChangePassword(string userName, string currentPassword, string newPassword);
		string CreateAccount(string userName, string password, bool requireConfirmationToken = false);
		bool UserExists(string userName);
	    bool ConfirmAccount(string confirmationToken);

		IPrincipal CurrentUser { get; }
	}
}