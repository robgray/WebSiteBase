using System;
using System.Diagnostics.Contracts;
using Castle.MicroKernel.Facilities;
using NHibernate;

namespace Infrastructure.Repositories.NHibernate
{
    /// <summary>
    /// 	The session manager is an object wrapper around the "real" manager which is managed
    /// 	by a custom per-transaction lifestyle. If you wish to implement your own manager, you can
    /// 	pass a function to this object at construction time and replace the built-in session manager.
    /// </summary>
    public class SessionManager : ISessionManager
    {
        private readonly Func<ISession> getSession;

        /// <summary>
        /// 	Constructor.
        /// </summary>
        /// <param name = "getSession"></param>
        public SessionManager(Func<ISession> getSession)
        {
            Contract.Requires(getSession != null);
            Contract.Ensures(this.getSession != null);

            this.getSession = getSession;
        }

        ISession ISessionManager.OpenSession()
        {
            var session = getSession();

            if (session == null)
                throw new FacilityException(
                    "The Func<ISession> passed to SessionManager returned a null session. Verify your registration.");

            return session;
        }
    }
}