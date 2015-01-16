using Mvc.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using WebBase.Mvc.Controllers;
using WebBase.Mvc.Plumbing;

namespace Mvc.Tests.Controllers
{
	[TestFixture]
	public class AccountControllerTests : WindsorControllerTestBase<AccountController> 
    {        
        [Test]
        public void Logout_logoff_user_out()
        {
            Controller.LogOff();

            Dep<IAuth>().Received(1).SignOut();            
        }

	    [Test]
	    public void Logout_redirects_to_home_page()
	    {            
            var response = Controller.LogOff();

	        response.AssertActionRedirect()
	                .ToController("Home")
	                .ToAction("Index");
	    }

    }
}
