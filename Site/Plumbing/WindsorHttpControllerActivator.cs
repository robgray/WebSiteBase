using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel;

namespace WebBase.Mvc.Plumbing
{
    public class WindsorHttpControllerActivator : IHttpControllerActivator
    {
        private readonly IKernel container;

        public WindsorHttpControllerActivator(IKernel container)
        {
            this.container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (this.container.HasComponent(controllerType))
            {
                return (IHttpController)this.container.Resolve(controllerType);
            }

            throw new ComponentNotFoundException(
                controllerType.ToString(), "Could not find component for " + controllerType);
        }
    }
}