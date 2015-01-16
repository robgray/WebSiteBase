using Castle.Transactions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Repositories.NHibernate.Mappings;
using Infrastructure.Repositories.NHibernate.Mappings.Conventions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Infrastructure.Repositories.NHibernate.Installers
{
	public class NHibernateInstaller : INHibernateInstaller
	{
		private readonly Maybe<IInterceptor> _Interceptor;

		public NHibernateInstaller()
		{
			_Interceptor = Maybe.None<IInterceptor>();
		}

        public NHibernateInstaller(IInterceptor interceptor)
        {
            _Interceptor = Maybe.Some(interceptor);
        }

        public Maybe<IInterceptor> Interceptor
        {
            get { return _Interceptor; }
        }

        public bool IsDefault
        {
            get { return true; }
        }

		public FluentConfiguration BuildFluent()
		{
		    return Fluently.Configure()
		                   .Database(SetupDatabase)
		                   .Mappings(m => m.FluentMappings
                                            .AddFromAssemblyOf<UserMap>()
                                            .Conventions.AddFromAssemblyOf<ReferenceIdNameConvention>());
		}

		protected virtual void ConfigurePersistence(Configuration config)
		{
			SchemaMetadataUpdater.QuoteTableAndColumns(config);
		}

		protected virtual IPersistenceConfigurer SetupDatabase()
		{
			return MsSqlConfiguration.MsSql2008.DefaultSchema("dbo")
                .ConnectionString(x => x.FromConnectionStringWithKey("Application"));				
		}

		public void Registered(ISessionFactory factory)
		{			

		}

		public string SessionFactoryKey
		{
			get { return "sf.default"; }
		}
	}
}
