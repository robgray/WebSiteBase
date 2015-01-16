using Castle.MicroKernel.Registration;
using FluentValidation;

namespace WebBase.Mvc.Installers
{
    public class ValidationsInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn(typeof (IValidator<>))
                                   .WithService.Base());
        }
    }
}