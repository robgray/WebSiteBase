using NHibernate;

namespace Infrastructure.Repositories.NHibernate
{
    /// <summary>
    /// 	Session manager interface. This denotes the ISession factory. The default
    /// 	session lifestyle is per-transaction, so call OpenSession within a transaction!
    /// </summary>    
    public interface ISessionManager
    {
        /// <summary>
        /// 	Gets a new or existing ISession depending on your context.
        /// </summary>
        /// <returns>A non-null ISession.</returns>
        ISession OpenSession();
    }
}