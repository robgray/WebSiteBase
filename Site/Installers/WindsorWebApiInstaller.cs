﻿using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using WebBase.Mvc.Plumbing;

namespace WebBase.Mvc.Installers
{
    public class WindsorWebApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IHttpControllerActivator>().ImplementedBy<WindsorHttpControllerActivator>());

            container.Register(AllTypes.FromThisAssembly().BasedOn<ApiController>().LifestyleTransient());
        }
    }
}