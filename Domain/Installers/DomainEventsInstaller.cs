using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Domain.Events;

namespace Domain.Installers
{
    public class DomainEventsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .BasedOn(typeof(IHandles<>))
                                      .WithService.Base()
                                      .LifestyleTransient());            
        }
    }
}
