using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Repositories.NHibernate.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace UnitTests.Repositories.Infrastructure
{
	public abstract class InMemoryDatabase : IDisposable
	{
		private static Configuration _confiugation;
		private static ISessionFactory _sessionFactory;

		protected ISession Session { get; set; }

		protected InMemoryDatabase()
		{
			_sessionFactory = CreateSessionFactory();
			Session = _sessionFactory.OpenSession();
			BuildSchema(Session);
		}

        protected void CreateNewSession()
        {
            _sessionFactory = CreateSessionFactory();
			Session = _sessionFactory.OpenSession();
			BuildSchema(Session);
        }

		private static ISessionFactory CreateSessionFactory()
		{

			return Fluently.Configure()
				.Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                //.Mappings(m => m.FluentMappings.Add<ClientMap>())
				.ExposeConfiguration(cfg => _confiugation = cfg)
				.BuildSessionFactory();
		}

		private static void BuildSchema(ISession session)
		{
			var export = new SchemaExport(_confiugation);            
			export.Execute(true, true, false, session.Connection, null);
		}

		public void Dispose()
		{
			Session.Dispose();
		}
	}
}
