
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.Windsor;
using NSubstitute;
using NUnit.Framework;

namespace Tests.Common
{    
    public abstract class WindsorTestBase<TService> where TService : class
    {
        private IWindsorContainer container;

        protected TService Service;

        [SetUp]
        public void SetUp()
        {
            container =
                new WindsorContainer().Register(
                    Component.For<ILazyComponentLoader>().ImplementedBy<LazyComponentAutoMocker>(),
                    Component.For<TService>());

            DoRegistrations();



            // build sut and inject mocks for all dependancies
            Service = container.Resolve<TService>();

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
