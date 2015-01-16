using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;
using Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace WebBase.Mvc.Plumbing.Providers
{
    public class NHibernateRoleProvider : RoleProvider
    {
        private string _connectionString;
        private static ISessionFactory _sessionFactory;
        
        private static ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name))
                name = "NHibernateRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "NHibernate Role Provider");
            }

            base.Initialize(name, config);

            // Initialize Connection.
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["Application"];
            if (connectionString == null || connectionString.ConnectionString.Trim() == "")
                throw new ProviderException("Connection string cannot be blank.");

            _connectionString = connectionString.ConnectionString;
            _sessionFactory = SessionHelper.CreateSessionFactory(_connectionString);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new InvalidOperationException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new InvalidOperationException();
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new InvalidOperationException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new InvalidOperationException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (!RoleExists(roleName))
                throw new Exception("Role " + roleName + " is not a valid role");

            Func<User, bool> usernamePredicate =
                (u => u.Username.Contains(usernameToMatch) || string.IsNullOrEmpty(usernameToMatch));

            using (var session = SessionFactory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    return new string[] {};
                }
            }

            return new string[] { };
        }

        public override string[] GetAllRoles()
        {
            return new string[] { };
        }

        public override string[] GetRolesForUser(string username)
        {
            var roles = new List<string>();
            using (var session = SessionFactory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    var user = session.Query<User>()
                                      .SingleOrDefault(x => x.Username == username);

                   
                }
            }

            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (!RoleExists(roleName))
                throw new Exception("Role " + roleName + " is not a valid role");

            using (var session = SessionFactory.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    return new string[] {};
                }
            }

            return new string[] {};
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRolesForUser(username).Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return GetAllRoles().Contains(roleName);
        }
    }
}