using System;
using Castle.Core.Internal;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using FizzioFit.Domain.Entities;
using FizzioFit.Domain.Entities.Plumbing;
using FizzioFit.Infrastructure.Repositories.NHibernate.Mappings;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace FizzioFit.Infrastructure.Repositories.NHibernate.Facilities
{
	public class PersistenceFacility : AbstractFacility 
	{
		protected override void Init()
		{
			var config = BuildDatabaseConfiguration();

			Kernel.Register(
				Component.For<ISessionFactory>()
					.UsingFactoryMethod(_ => config.BuildSessionFactory()),
				Component.For<ISession>()
					.UsingFactoryMethod(k => k.Resolve<ISessionFactory>().OpenSession())
					.LifestylePerWebRequest()
				);
		}
		
		protected virtual bool IsDomainEntity(Type t)
		{
			return typeof(IHasId).IsAssignableFrom(t);
		}

		private void ShouldIgnoreProperty(IPropertyIgnorer property)
		{
			property.IgnoreProperties(p => p.MemberInfo.HasAttribute<DoNotMapAttribute>());
		}

		private Configuration BuildDatabaseConfiguration()
		{
			return Fluently.Configure()
				.Database(SetupDatabase)
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ClientMap>())
				.ExposeConfiguration(ConfigurePersistence)
				.BuildConfiguration();
		}

		protected virtual IPersistenceConfigurer SetupDatabase()
		{
		    return MsSqlConfiguration.MsSql2008
		                             .UseOuterJoin()
		                             .ConnectionString(x => x.FromConnectionStringWithKey("FizzioFit"))
		                             .ShowSql();
		}

		protected virtual void ConfigurePersistence(Configuration config)
		{
			SchemaMetadataUpdater.QuoteTableAndColumns(config);
		}

	}
}
