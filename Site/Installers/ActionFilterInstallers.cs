using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace WebBase.Mvc.Installers
{
    public class ActionFilterInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .InNamespace("Mvc.Filters")
                                      .LifestyleTransient());
        }
    }
}