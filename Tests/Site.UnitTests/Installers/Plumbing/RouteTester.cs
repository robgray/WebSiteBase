using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using NUnit.Framework;
using WebBase.Mvc.App_Start;

namespace Mvc.Tests.Installers.Plumbing
{    
    public class RouteTester
    {
        HttpConfiguration config;
        HttpRequestMessage request;
        private IHttpRouteData routeData;
        private HttpControllerContext controllerContext;
        private IHttpControllerSelector controllerSelector;

        public RouteTester(HttpConfiguration conf, HttpRequestMessage req)
        {
            config = conf;
            request = req;
            routeData = config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controllerSelector = new DefaultHttpControllerSelector(config);
            controllerContext = new HttpControllerContext(config, routeData, request);
        }

        public string GetActionName()
        {
            if (controllerContext.ControllerDescriptor == null)
                GetControllerType();

            var actionSelector = new ApiControllerActionSelector();
            var descriptor = actionSelector.SelectAction(controllerContext);

            return descriptor.ActionName;
        }

        public Type GetControllerType()
        {
            var descriptor = controllerSelector.SelectController(request);
            controllerContext.ControllerDescriptor = descriptor;

            return descriptor.ControllerType;
        }

        public void AssertController<T>()
        {
            Assert.AreEqual(typeof(T), GetControllerType());
        }

        public void AssertActionName<T>(Expression<Func<T, object>> expression)
        {            
            Assert.AreEqual(ReflectionHelpers.GetMethodName(expression).ToLower(), GetActionName().ToLower());
        }
    }

    public static class RouteTesterHelper
    {
        public static RouteTester ForController<T>(this HttpRequestMessage request)
        {
            var config = new HttpConfiguration();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Routes.Clear();

            WebApiConfig.Register(config);

            var routeTester = new RouteTester(config, request);

            routeTester.AssertController<T>();

            return routeTester;
        }

        public static void ForActionName<T>(this RouteTester routeTester, Expression<Func<T, object>> expression)
        {
            routeTester.AssertActionName<T>(expression);   
        }    
    }
}
