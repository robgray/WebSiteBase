using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using WebBase.Mvc.App_Start;
using log4net;
using log4net.Config;

namespace Mvc.Tests.Conventions
{
    public class List_misconfigured_components_in_Windsor :
       WindsorConventionTest<IPotentiallyMisconfiguredComponentsDiagnostic>
    {
        protected override WindsorConventionData<IHandler> SetUp()
        {
            var container = CreateAndConfigureContainerLikeInTheApp();
            return
                new WindsorConventionData<IHandler>(container)
                {
                    FailDescription = "The following components come up as potentially unresolvable",
                    FailItemDescription = MisconfiguredComponentDescription
                }.WithApprovedExceptions("it's *potentially* for a reason");
        }

        private IWindsorContainer CreateAndConfigureContainerLikeInTheApp()
        {
            XmlConfigurator.Configure();

            var appLogger = LogManager.GetLogger(GetType());
            AssembliesConfig.LookInBinSubfolder = false; // NOTE: this is temporary until we remove that code completely
            var modules = AssembliesConfig.Configure(appLogger);
            var container = ContainerConfig.Configure(appLogger);


            return container;
        }
    }
}
