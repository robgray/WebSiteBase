using Castle.Windsor.Diagnostics.Helpers;
using NUnit.Framework;
using WebBase.Mvc.App_Start;
using log4net;
using log4net.Config;

namespace Mvc.Tests.Conventions
{
    [Ignore]
    public class List_all_the_components_registered_in_Windsor : WindsorConventionTest
    {
        protected override WindsorConventionData SetUp()
        {
            XmlConfigurator.Configure();

            AppLogger = LogManager.GetLogger(GetType());
            AssembliesConfig.LookInBinSubfolder = false; // NOTE: this is temporary until we remove that code completely
            var assmebliesLoaded = AssembliesConfig.Configure(AppLogger);
            var container = ContainerConfig.Configure(AppLogger);

            return new WindsorConventionData(container)
            {
                FailDescription = "All Windsor components",
                FailItemDescription = h => BuildDetailedHandlerDescription(h) + " | " +
                                           h.ComponentModel.GetLifestyleDescriptionLong(),
                HasApprovedExceptions = true
            };
        }

        protected ILog AppLogger { get; set; }
    }
}
