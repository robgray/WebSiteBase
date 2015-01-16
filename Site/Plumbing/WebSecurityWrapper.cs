using System;
using WebMatrix.WebData;

namespace WebBase.Mvc.Plumbing
{
	public class WebSecurityWrapper : IWebSecurity
	{
		public bool Login(string userName, string password, bool persistCookie = false)
		{
			return WebSecurity.Login(userName, password, persistCookie);
		}

		public void Logout()
		{
			WebSecurity.Logout();
		}

		public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
		{
			return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);			
		}

		public int GetUserId(string userName)
		{
			return WebSecurity.GetUserId(userName);
		}

		public bool ChangePassword(string userName, string currentPassword, string newPassword)
		{
			return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
		}

		public string CreateAccount(string userName, string password, bool requireConfirmationToken = false)
		{
			return WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
		}

		public bool UserExists(string userName)
		{
			//return WebSecurity.UserExists(userName);
		    return WebSecurity.IsConfirmed(userName);
		}

        public bool ConfirmAccount(string confirmationToken)
        {
            return WebSecurity.ConfirmAccount(confirmationToken);
        }

		public System.Security.Principal.IPrincipal CurrentUser
		{
			get { throw new NotImplementedException(); }
		}
	}
}