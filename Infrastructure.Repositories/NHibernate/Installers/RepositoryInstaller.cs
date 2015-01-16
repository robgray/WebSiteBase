using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Domain.Interfaces;
using Infrastructure.Common.Windsor;

namespace Infrastructure.Repositories.NHibernate.Installers
{
	public class RepositoryInstaller : IWindsorInstaller  
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{		    
            // DefaultInterfaces() registers the Service as the Interface that inherits from IRepository<>
            // Take the following:
            //
            // public interface IProgramRepository : IRepository<Program> { }
            // public class ProgramRepository : IProgramRepository { }
            //
            // Then ProgramRepository is the component resolved from the service IProgramRepository.

		    container.Register(Classes.FromThisAssembly()
		                              .BasedOn(typeof (IRepository<>))
		                              .WithService.DefaultInterfaces()
                                      .LifestylePerWebRequestOrScoped());		   
		}
	}
}
