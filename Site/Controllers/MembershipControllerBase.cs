using System.Web.Mvc;
using System.Web.Security;
using Domain.Interfaces;
using WebBase.Mvc.Helpers;
using WebBase.Mvc.Plumbing;

namespace WebBase.Mvc.Controllers
{
    public class MembershipControllerBase  : ControllerBase
    {
        public const int AvatarFileSizeLimit = 1024 * 1024; //1mb
        public static readonly string[] ValidAvatarContentTypes = new string[] { "image/jpeg", "image/png" };

        public IAuth AuthenticationProvider { get; set; }

        public ICryptographyProvider CryptographyProvider { get; set; }  // injected.

        protected static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Email address is already in use.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        public ActionResult UploadErrorResult(string message)
        {
            return new SimpleTextResult("ERROR:" + message);
        }

        public ActionResult UploadSuccessResult(string message)
        {
            return new SimpleTextResult("OK:" + message);
        }
    
    }
}