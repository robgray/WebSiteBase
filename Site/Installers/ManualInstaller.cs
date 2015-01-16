using System.Web.Mvc;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.Common;
using WebBase.Mvc.Plumbing;
using WebBase.Mvc.Services;

namespace WebBase.Mvc.Installers
{    
    /// <summary>
    /// Use this installer to manually register services with the container
    /// </summary>
    public class ManualInstaller  : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(                
                Component.For<IEmailService>().UsingFactoryMethod<IEmailService>((k, c) =>
                {
                    var config = k.Resolve<IConfigurationManager>();
                    if (config.UseEmailService)
                    {
                        return new NullEmailService(k.Resolve<ILogger>());
                    }
                    // We've only gor the NullEmailService...
                    return new NullEmailService(k.Resolve<ILogger>());
                }).LifestyleTransient(),
                Component.For<IActionInvoker>().Instance(new WindsorActionInvoker(container)),
                Component.For<IAuth>().ImplementedBy<FormsAuthenticationWrapper>()                
            );
        }
    }  
}