using System;
using Castle.Core.Logging;
using Infrastructure.Repositories.NHibernate.Installers;

namespace WebBase.Mvc.App_Start.StartupTests
{
    public class NHibernateInstallerHasInterceptor //: IStartupTest
    {
        public INHibernateInstaller Installer { get; set; }

        public ILogger Logger { get; set; }

        public void RunTest()
        {
            if (Installer == null)
                throw new Exception("NHibernate Installer was not found!");

            if (!Installer.Interceptor.HasValue)
                throw new Exception("NHibernate Installer Interceptor was not there! :S");

        }
    }
}