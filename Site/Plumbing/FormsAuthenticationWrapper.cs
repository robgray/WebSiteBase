using System.Web.Security;

namespace WebBase.Mvc.Plumbing
{
    public interface IAuth
    {
        void DoAuth(string username, bool remember);

        void SignOut();
    }

    public class FormsAuthenticationWrapper : IAuth
    {
        public void DoAuth(string username, bool remember)
        {
            FormsAuthentication.SetAuthCookie(username, remember);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
