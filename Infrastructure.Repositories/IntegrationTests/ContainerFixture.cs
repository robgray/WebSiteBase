using System;
using System.Transactions;
using Castle.Facilities.AutoTx;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FizzioFit.Infrastructure.Repositories.NHibernate;
using FizzioFit.Infrastructure.Repositories.NHibernate.Facilities;
using FizzioFit.Infrastructure.Repositories.NHibernate.Installers;
using FizzioFit.Mvc;
using NUnit.Framework;

namespace IntegrationTests
{
    public class ContainerFixture
    {
        protected IWindsorContainer Container { get; set;  }

        public ContainerFixture()
        {
            // Recreate the database on startup.
            // This is an expensive operation.  But's in an intergration test, so... ya know... ;-)
            DbUpdateConfig.UpgradeDatabase(dropAndRecreateArgs: "");

            // Configure the container 
            Container = new WindsorContainer();

            Container.AddFacility<AutoTxFacility>();
            Container.Register(Component.For<INHibernateInstaller>().ImplementedBy<NHibernateInstaller>().LifeStyle.Singleton);
			Container.AddFacility<NHibernateFacility>(f => { f.DefaultLifeStyle = DefaultSessionLifeStyleOption.SessionTransient; f.ShowSql = true; });

            //Container.Register(Classes.FromAssemblyContaining<IProgramRepository>()
            //                          .BasedOn(typeof(IRepository<>))
            //                          .WithService.DefaultInterfaces()
            //                          .LifestyleTransient());
            //Container.Install(FromAssembly.Containing<ClientValidator>());            
            //Container.Install(FromAssembly.Containing<PersonNameParser>());


#if DEBUG
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif
   
        }	    
    }
}
