using System.Web.Routing;
using Mvc.Tests.Helpers;
using NUnit.Framework;
using WebBase.Mvc.App_Start;
using WebBase.Mvc.Controllers;

namespace Mvc.Tests.Installers.Plumbing
{
    [TestFixture]
    public class Route_Tests
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            var routes = RouteTable.Routes;
            routes.Clear();

            RouteConfig.RegisterRoutes(routes);            
        }

        [Test]
        public void Home_Index()
        {
            "~/".ShouldMapTo<HomeController>(action => action.Index(null));
        }        
    }
}
