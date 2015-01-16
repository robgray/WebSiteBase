using System.Web.Mvc;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.Windsor;
using Domain.Interfaces;
using Mvc.Tests.Mocks;
using NSubstitute;
using NUnit.Framework;
using Tests.Common;
using WebBase.Mvc.App_Start;
using ControllerBase = WebBase.Mvc.Controllers.ControllerBase;

namespace Mvc.Tests.Controllers
{
    public class WindsorControllerTestBase<TService> where TService : ControllerBase
    {
        private IWindsorContainer container;

        protected TService Controller;

        [SetUp]
        public void SetUp()
        {
            var contextBase = MvcMockHelpers.FakeHttpContext();
            container = new WindsorContainer();
            container.AddFacility<FactorySupportFacility>();
            container.Register(Component.For<UrlHelper>().LifeStyle.Transient
                                        .UsingFactoryMethod(() =>
                                            {
                                                var routes = new RouteCollection();
                                                RouteConfig.RegisterRoutes(routes);
                                                var routeData = RouteTable.Routes.GetRouteData(contextBase);
                                                return new UrlHelper(new RequestContext(contextBase, routeData ?? new RouteData()));
                                            }));
            container.Register(Component.For<ILazyComponentLoader>().ImplementedBy<LazyComponentAutoMocker>());
            container.Register(Component.For<TService>());

            //Register<IWebSecurity>(Substitute.For<IWebSecurity>());
            //Register<IOAuthWebSecurity>(Substitute.For<IOAuthWebSecurity>());
            //Register<IRoleService>(Substitute.For<IRoleService>());
            Register<IUserRepository>(Substitute.For<IUserRepository>());            
            Register<ILogger>(Substitute.For<ILogger>());

            DoRegistrations();
                        
            // build sut and inject mocks for all dependancies
            Controller = container.Resolve<TService>();
            Controller.SetFakeControllerContext();


            //Controller.WebSecurity = Dep<IWebSecurity>();
            //Controller.OAuthWebSecurity = Dep<IOAuthWebSecurity>();
            //Controller.RoleService = Dep<IRoleService>();
            Controller.Logger = Dep<ILogger>();
            Controller.UserRepository = Dep<IUserRepository>();

            DoSetUp();        
        }

        protected virtual void DoRegistrations() { }

        protected virtual void DoSetUp() { }

        [TearDown]
        public void TearDown()
        {
            DoTearDown();

            container.Dispose();
        }

        protected virtual void DoTearDown() { }

        protected TDependency Dep<TDependency>()
        {
            return container.Resolve<TDependency>();
        }

        protected TMock Mock<TMock>() where TMock : class
        {
            return Substitute.For<TMock>();
        }

        protected void Register<TMock>(TMock instance) where TMock : class
        {
            container.Register(Component.For<TMock>().Instance(instance));
        }
    }
}
