using System;
using Castle.Facilities.FactorySupport;
using Castle.Facilities.Logging;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Infrastructure.Common.Installers;
using log4net;

namespace WebBase.Mvc.App_Start
{	
	public static class ContainerConfig
	{
		private static IWindsorContainer _container;

		public static IWindsorContainer Configure(ILog logger)
		{
			try {
				_container = new WindsorContainer();
				_container.AddFacility<LoggingFacility>(x => x.UseLog4Net());
                _container.AddFacility<TypedFactoryFacility>();
                _container.AddFacility<FactorySupportFacility>();
				_container.Install(FromAssembly.This());

                // I shouldn't need to do this, as Assemblies.Config should be loading into the AppDomain :S
                _container.Install(FromAssembly.Containing<UtilitiesInstaller>());			    
                _container.Install(FromAssembly.Containing<Domain.Installers.ManualInstaller>());
                _container.Install(FromAssembly.Containing<Infrastructure.Repositories.NHibernate.Installers.RepositoryInstaller>());
                
				return _container;
			}
			catch (Exception e) {
				logger.Fatal("Could not properly initialize container.", e);
				throw;
			}
		}
	}
}