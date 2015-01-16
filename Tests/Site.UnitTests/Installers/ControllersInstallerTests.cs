using System.Linq;
using System.Web.Mvc;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Windsor;
using NUnit.Framework;
using WebBase.Mvc.Controllers;
using WebBase.Mvc.Installers;

namespace Mvc.Tests.Installers
{
	[TestFixture]
	public class ControllersInstallerTests : InstallerTests
	{
		private IWindsorContainer _containerWithControllers;

		[SetUp]
		public void SetUp()
		{
			_containerWithControllers = new WindsorContainer().Install(new ControllersInstaller());
		}

		[Test]
		public void All_controllers_implement_IController()
		{
			var allHandlers = GetAllHandlers(_containerWithControllers);
			var controllerHandlers = GetHandlersFor(typeof (IController), _containerWithControllers);

			Assert.IsNotEmpty(allHandlers);
			Assert.AreEqual(allHandlers, controllerHandlers);
		}

		[Test]
		public void All_controllers_are_registered()
		{
			// Is<TType> is an helper, extension method from Windsor in the Castle.Core.Internal namespace
			// which behaves like 'is' keyword in C# but at a Type, not instance level
			var allControllers = GetPublicClassesFromApplicationAssembly<HomeController>(c => c.Is<IController>());
			var registeredControllers = GetImplementationTypesFor(typeof(IController), _containerWithControllers);
			Assert.AreEqual(allControllers, registeredControllers);
		}
		
		[Test]
		public void All_and_only_controllers_live_in_Controllers_namespace()
		{
			var allControllers = GetPublicClassesFromApplicationAssembly<HomeController>(c => c.Namespace.Contains("Controllers"));
			var registeredControllers = GetImplementationTypesFor(typeof(IController), _containerWithControllers);
			Assert.AreEqual(allControllers, registeredControllers);
		}

		[Test]
		public void All_controllers_are_transient()
		{
			var nonTransientControllers = GetHandlersFor(typeof(IController), _containerWithControllers)				
				.Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
				.ToArray();

			Assert.IsEmpty(nonTransientControllers);
		}

		[Test]
		public void All_controllers_expose_themselves_as_service()
		{
			var controllersWithWrongName = GetHandlersFor(typeof(IController), _containerWithControllers)
				.Where(controller => controller.ComponentModel.Services.Single() != controller.ComponentModel.Implementation)
				.ToArray();

			Assert.IsEmpty(controllersWithWrongName);
		}		
	}
}
