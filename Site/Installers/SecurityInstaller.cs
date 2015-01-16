using Castle.MicroKernel.Registration;
using WebBase.Mvc.Plumbing;

namespace WebBase.Mvc.Installers
{
	public class SecurityInstaller : IWindsorInstaller
	{
		public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
		{			
			container.Register(Component.For<IWebSecurity>().ImplementedBy<WebSecurityWrapper>().LifeStyle.Singleton);			
		}
	}
}