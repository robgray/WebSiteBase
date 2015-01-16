using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Common.Windsor;

namespace Domain.Installers
{
    public class ManualInstaller : IWindsorInstaller
    {
	    public void Install(IWindsorContainer container, IConfigurationStore store)
	    {
		    container.Register(Component.For<ICryptographyProvider>().ImplementedBy<CryptographyProvider>());		    		    
            container.Register(Component.For<IUserAvatarService>().ImplementedBy<UserAvatarService>().LifestylePerWebRequestOrScoped());
	    }
    }
}
