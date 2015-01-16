using FluentNHibernate.Cfg;
using Infrastructure.Repositories.NHibernate.Mappings;
using Infrastructure.Repositories.NHibernate.Mappings.Conventions;
using NHibernate;

namespace WebBase.Mvc.Plumbing.Providers
{
    internal static class SessionHelper
    {
        public static ISessionFactory CreateSessionFactory(string connstr)
        {
            return Fluently.Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(connstr))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>().Conventions.AddFromAssemblyOf<ReferenceIdNameConvention>())                
                .BuildSessionFactory();
        }
    }
}