using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace WebBase.Mvc.Plumbing
{
    internal class WindsorDependencyScope : IDependencyScope
    {
        private readonly List<object> instances = new List<object>();
        private readonly Action<object> releaseAction;
        private readonly IDependencyResolver resolver;

        public WindsorDependencyScope(IDependencyResolver resolver, Action<object> releaseAction)
        {
            this.resolver = resolver;
            this.releaseAction = releaseAction;
        }

        public object GetService(Type serviceType)
        {
            var service = this.resolver.GetService(serviceType);
            if (service != null)
            {
                this.instances.Add(service);
            }
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var services = this.resolver.GetServices(serviceType).ToList();
            this.instances.AddRange(services);
            return services;
        }

        public void Dispose()
        {
            foreach (var item in this.instances)
            {
                this.releaseAction(item);
            }
        }
    }
}