﻿using System.Web.Mvc;
using System.Web.Routing;

namespace WebBase.Mvc.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
		    routes.LowercaseUrls = true;

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.Ignore("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});

		    routes.MapRoute(
		        name: "Default",
		        url: "{controller}/{action}/{id}",
		        defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional});		    
		}
	}
}