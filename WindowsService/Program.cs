using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Configuration.Install;
using Castle.Windsor;
using WebBase.Mvc.App_Start;
using log4net;
using log4net.Config;

namespace WindowsService
{
    class Program
    {
        private static IWindsorContainer Container;
        protected static ILog Logger { get; set; }

        static void Main(string[] args)
        {            
            XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(Program));

            Logger.Info("Loading Assemblies");
            AssembliesConfig.LookInBinSubfolder = false;
            AssembliesConfig.Configure(Logger);

            Logger.Info("Bootstrapping the IoC container...");
            Container = WindsorConfig.Configure(Logger);

            string allArgs = string.Join(" ", args ?? new string[0]);

            // Installer
            if (Environment.UserInteractive)
            {                
                if ("--install".Equals(allArgs, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Info("Installing as a Service");
                    new AssemblyInstaller(Assembly.GetExecutingAssembly(), null).Install(new Hashtable());
                    return;
                }

                if ("--uninstall".Equals(allArgs, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.Info("UnInstalling Service");
                    new AssemblyInstaller(Assembly.GetExecutingAssembly(), null).Uninstall(new Hashtable());
                    return;
                }
            }

            // Initialize environment
            var services = Container.ResolveAll<ServiceBase>();

            if (Environment.UserInteractive)
            {
                RunInteractive(services);               
            }
            else
            {
                // Service version
                ServiceBase.Run(services);
            }
        }

        static void RunInteractive(ServiceBase[] servicesToRun)
        {
            if (!servicesToRun.Any())
            {
                Logger.Warn("No services found. Stopping...");
            }
            else
            {
                Logger.Info("Services running in interactive mode.");

                var stopEvent = new ManualResetEvent(false);
                Console.CancelKeyPress += (sender, eventArgs) =>
                    {
                        stopEvent.Set();
                        eventArgs.Cancel = true;
                    };

                MethodInfo onStartMethod = typeof (ServiceBase).GetMethod("OnStart",
                                                                          BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (ServiceBase service in servicesToRun)
                {
                    Logger.InfoFormat("Starting {0}...", service.ServiceName);
                    onStartMethod.Invoke(service, new object[] {new string[] {}});
                    Logger.Info("Started");
                }

                Logger.Info("Press Ctrl+C to stop");

                stopEvent.WaitOne();

                MethodInfo onStopMethod = typeof (ServiceBase).GetMethod("OnStop",
                                                                         BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (ServiceBase service in servicesToRun)
                {
                    Logger.InfoFormat("Stopping {0}...", service.ServiceName);
                    onStopMethod.Invoke(service, null);
                    Logger.Info("Stopped");

                }

                Logger.Info("All Services stopped.");
            }

            // keep open for 1 second so user can see msg.
            Thread.Sleep(1000);
        }
    }
}
