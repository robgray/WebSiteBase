using System.ServiceProcess;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace WindowsService.Installers
{
    public class ServicesInstaller  : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {            
            // Task Manager has it's own Installer, with task related classes
            container.Register(Classes.FromThisAssembly()
                .Where(t => t.BaseType == typeof(ServiceBase) && !t.IsAssignableFrom(typeof(TaskManager)))
                .WithServiceBase());            
        }
    }
}
