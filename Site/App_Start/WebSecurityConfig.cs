using System;
using WebMatrix.WebData;

namespace WebBase.Mvc.App_Start
{
	public static class WebSecurityConfig
	{
		public static void Initialize()
		{
			try 
			{
				WebSecurity.InitializeDatabaseConnection("Application", "User", "UserId", "UserName", autoCreateTables: false);
			}
				catch (Exception ex) {
					throw new InvalidOperationException(
						"The ASP.NET Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
			}
		}
	}
}