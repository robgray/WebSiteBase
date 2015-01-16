using System.Linq;
using System.ServiceProcess;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.Common.Windsor;
using WindowsService.Tasks;

namespace WindowsService.Installers
{
    public class TasksInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {                        
            container.Kernel.ComponentModelBuilder.AddContributor(new LifestyleScopeInterceptorContributor());
            container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<TaskSelector>()
                               //, Component.For<AutoReleaseHandlerInterceptor>()
                               , Component.For<LifestyleScopeInterceptor>()
                               , Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
                               , Classes.FromThisAssembly()
                                        .BasedOn<ITaskDefinition>()
                                        .WithService.Base().LifestyleSingleton()
                                , Classes.FromThisAssembly()
                                        .BasedOn<IWorker>()                                        
                                        .WithService.Self().LifestyleTransient()
                                        
                               , Component.For<ServiceBase>().ImplementedBy<TaskManager>()
                                                             //.Interceptors<AutoReleaseHandlerInterceptor>()
                               , Component.For<TaskDefinitionFactory>().AsFactory());
            
            
            
            
            
            
            
            
            
            // Later on if we need environment specific tasks we could create an attribute for each environment
            // and a config setting, then select components based on the config setting and the attribute
            // e.g.
            //
            //container.Register(Classes.FromThisAssembly()            
            //                          .Where(t => Attribute.IsDefined(t, typeof (OnProductionAttribute)))
            //                          .WithService.DefaultInterfaces().LifestyleSingleton());
        }
    }

    public class LifestyleScopeInterceptorContributor : IContributeComponentModelConstruction
    {        
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (model.Services.Any(t => t == typeof (ITaskDefinition)))
            {
                model.Interceptors.Add(new InterceptorReference(typeof (LifestyleScopeInterceptor)));
            }
        }
    }
}
