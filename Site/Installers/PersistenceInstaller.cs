using Castle.Facilities.AutoTx;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Infrastructure.Repositories.NHibernate;
using Infrastructure.Repositories.NHibernate.Facilities;
using Infrastructure.Repositories.NHibernate.Installers;

namespace WebBase.Mvc.Installers
{
    public class PersistenceInstaller : IWindsorInstaller
    {
	    public void Install(IWindsorContainer container, IConfigurationStore store)
	    {
		    container.AddFacility<AutoTxFacility>();            
		    container.Register(Component.For<INHibernateInstaller>().ImplementedBy<NHibernateInstaller>().LifeStyle.Singleton);
	        container.AddFacility<NHibernateFacility>(f => f.DefaultLifeStyle = DefaultSessionLifeStyleOption.SessionPerWebRequest);                            
	    }
    }
}