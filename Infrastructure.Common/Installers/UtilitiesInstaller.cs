using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Infrastructure.Common.Installers
{
    public class UtilitiesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationManager>()
                                        .ImplementedBy<ConfigurationManager>()
                                        .LifeStyle.Singleton,
                               Component.For<ITransactioner>().ImplementedBy<Transactioner>().LifestyleTransient());
        }
    }
}
