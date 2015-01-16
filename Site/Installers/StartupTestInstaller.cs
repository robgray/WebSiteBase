using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using WebBase.Mvc.App_Start.StartupTests;

namespace WebBase.Mvc.Installers
{
	public class StartupTestInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Classes.FromThisAssembly().BasedOn<IStartupTest>().WithServiceBase()
				);
		}
	}
}