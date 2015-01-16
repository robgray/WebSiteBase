using System.Security.Principal;
using Domain.Entities;

namespace WebBase.Mvc.Plumbing
{
    public class UserPrincipal : IPrincipal
    {
        readonly IIdentity _identity;
        readonly User _user;

        public UserPrincipal(User user)
        {
            _user = user;
            _identity = new GenericIdentity(user.EmailAddress);
        }

        /// <summary>
        /// Determines whether the User has the specified Role
        /// </summary>
        public bool IsInRole(string role)
        {
            return true;
        }

        /// <summary>
        /// Customer associated with this Principal
        /// </summary>
        public User User
        {
            get
            {
                return _user;
            }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }
    }

    /// <summary>
    /// Represents an anonymous user session
    /// HttpContext sometimes requires access to the identity (eg in TextImage)
    /// when attempting to set the cachability of a response
    /// This is the default identity (anonymous) for when the Customer is not logged in
    /// </summary>
    public class AnonymousPrincipal : IPrincipal
    {
        readonly AnonymousIdentity _identity = new AnonymousIdentity();

        public bool IsInRole(string role)
        {
            return false;
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        private class AnonymousIdentity : IIdentity
        {
            public string Name
            {
                get { return "Anonymous"; }
            }

            public string AuthenticationType
            {
                get { return string.Empty; }
            }

            public bool IsAuthenticated
            {
                get { return false; }
            }
        }
    }
}