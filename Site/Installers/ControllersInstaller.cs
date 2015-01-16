using System.Web.Mvc;
using Castle.MicroKernel.Registration;

namespace WebBase.Mvc.Installers
{
	public class ControllersInstaller : IWindsorInstaller
	{
		public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
		{
			container.Register(Classes.FromThisAssembly()
				                   .BasedOn<IController>()
				                   .LifestyleTransient());
		}
	}
}