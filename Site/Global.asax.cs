using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Core.Logging;
using Castle.Windsor;
using FluentValidation.Mvc;
using WebBase.Mvc.App_Start;
using WebBase.Mvc.App_Start.StartupTests;
using WebBase.Mvc.Plumbing;
using log4net;
using log4net.Config;

namespace WebBase.Mvc
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		private static IWindsorContainer Container;
		protected ILog AppLogger { get; set; }

		protected void Application_Start()
		{
			AppDomain.CurrentDomain.UnhandledException += Application_Error;

			try {

				XmlConfigurator.Configure();
				AppLogger = LogManager.GetLogger(GetType());
				AppLogger.Info("Application_Start...");

				AppLogger.Info("Ensuring Assembles to Scan are loaded...");
				AssembliesConfig.Configure(AppLogger);

				AppLogger.Info("Bootstrapping the container...");
				Container = ContainerConfig.Configure(AppLogger);
				
				AppLogger.Info("Setting the Controller Factory...");
				var controllerFactory = new WindsorControllerFactory(Container.Kernel);
				ControllerBuilder.Current.SetControllerFactory(controllerFactory);

				AppLogger.Info("Register All Areas...");
				AreaRegistration.RegisterAllAreas();

				AppLogger.Info("Register WebApi...");
				WebApiConfig.Register(GlobalConfiguration.Configuration);

				AppLogger.Info("Register Global Filters...");
				FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

				AppLogger.Info("Register Routes...");
				RouteConfig.RegisterRoutes(RouteTable.Routes);

				AppLogger.Info("Register Bundles...");
				BundleConfig.RegisterBundles(BundleTable.Bundles);
				AuthConfig.RegisterAuth();

                //  This needs to execute before anything that touches the database.			    
                DbUpdateConfig.UpgradeDatabase();

				AppLogger.Info("Bootstrap Security...");
				WebSecurityConfig.Initialize();

				AppLogger.Info("Running startup tests...");
				foreach (var test in Container.ResolveAll<IStartupTest>()) {
					test.RunTest();
				}

                AppLogger.Info("Configuring Fluent Validators...");
			    FluentValidationModelValidatorProvider.Configure(
			        provider => { provider.ValidatorFactory = new WindsorFluentValidatorFactory(Container); });

                RegisterDependencyResolver();

			} catch (Exception ex) {
				AppLogger.Fatal("Application_Start FAILED", ex);
				throw;
			}
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			var exception = Server.GetLastError();

			// Don't log client http errors.
			var httpException = exception as HttpException;
			if (httpException != null && httpException.GetHttpCode() >= 400 && httpException.GetHttpCode() <= 499)
				return;

			if (AppLogger != null) {
				AppLogger.Error("Exception caught in global exception handler.", Server.GetLastError());
			}
		}
		
		protected void Application_End()
		{
			try {
				var logger = Container.Resolve<ILogger>();
				if (logger != null)
					logger.Info("Application_End...");
			}
			finally {
				Container.Dispose();
			}
		}

        protected void RegisterDependencyResolver()
        {
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(Container.Kernel);
        }
	}
}